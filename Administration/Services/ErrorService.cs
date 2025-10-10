using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Administration.Services
{
    public interface IErrorService
    {
        Task LogErrorAsync(Exception exception, HttpContext? context = null, string? additionalInfo = null);
        Task<string> GetUserFriendlyMessageAsync(Exception exception);
        Task NotifyAdminAsync(Exception exception, HttpContext? context = null);
        string GenerateErrorId();
        Task SaveErrorDetailsAsync(string errorId, Exception exception, HttpContext? context = null);
    }

    public class ErrorService : IErrorService
    {
        private readonly ILogger<ErrorService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public ErrorService(ILogger<ErrorService> logger, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        public async Task LogErrorAsync(Exception exception, HttpContext? context = null, string? additionalInfo = null)
        {
            var errorId = GenerateErrorId();
            
            var errorDetails = new
            {
                ErrorId = errorId,
                Timestamp = DateTime.UtcNow,
                Exception = new
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                },
                Context = context != null ? new
                {
                    Path = context.Request.Path.ToString(),
                    Method = context.Request.Method,
                    QueryString = context.Request.QueryString.ToString(),
                    UserAgent = context.Request.Headers["User-Agent"].ToString(),
                    User = context.User?.Identity?.Name,
                    IPAddress = context.Connection.RemoteIpAddress?.ToString()
                } : null,
                AdditionalInfo = additionalInfo,
                Environment = _environment.EnvironmentName
            };

            _logger.LogError(exception, 
                "?? Erreur {ErrorId} - {ExceptionType}: {Message} | Context: {Context}", 
                errorId, exception.GetType().Name, exception.Message, 
                context != null ? $"{context.Request.Method} {context.Request.Path}" : "N/A");

            // Sauvegarder les détails pour analyse ultérieure
            await SaveErrorDetailsAsync(errorId, exception, context);

            // Notifier les administrateurs pour les erreurs critiques
            if (IsCriticalError(exception))
            {
                await NotifyAdminAsync(exception, context);
            }
        }

        public Task<string> GetUserFriendlyMessageAsync(Exception exception)
        {
            var message = exception switch
            {
                ArgumentNullException => "Il manque des informations requises pour traiter votre demande.",
                ArgumentException => "Les informations fournies ne sont pas valides.",
                UnauthorizedAccessException => "Vous n'avez pas l'autorisation d'accéder à cette ressource.",
                KeyNotFoundException => "La ressource demandée est introuvable.",
                FileNotFoundException => "Le fichier demandé est introuvable.",
                InvalidOperationException => "Cette opération n'est pas possible dans le contexte actuel.",
                TimeoutException => "L'opération a pris trop de temps à s'exécuter. Veuillez réessayer.",
                Microsoft.EntityFrameworkCore.DbUpdateException => "Erreur lors de la sauvegarde. Vérifiez vos données et réessayez.",
                SqlException sqlEx => sqlEx.Number switch
                {
                    2 => "Impossible de se connecter à la base de données. Contactez l'administrateur.",
                    18456 => "Erreur d'accès à la base de données. Contactez l'administrateur.",
                    _ => "Erreur de base de données. Contactez l'administrateur si le problème persiste."
                },
                _ => _environment.IsDevelopment() 
                    ? exception.Message 
                    : "Une erreur inattendue s'est produite. Contactez l'administrateur si le problème persiste."
            };

            return Task.FromResult(message);
        }

        public Task NotifyAdminAsync(Exception exception, HttpContext? context = null)
        {
            try
            {
                var errorId = GenerateErrorId();
                var notification = new
                {
                    ErrorId = errorId,
                    Timestamp = DateTime.UtcNow,
                    Severity = GetSeverityLevel(exception),
                    Application = "Administration Animalia",
                    Environment = _environment.EnvironmentName,
                    Exception = exception.GetType().Name,
                    Message = exception.Message,
                    Path = context?.Request.Path.ToString(),
                    User = context?.User?.Identity?.Name,
                    UserAgent = context?.Request.Headers["User-Agent"].ToString()
                };

                _logger.LogCritical("?? NOTIFICATION ADMIN - Erreur critique détectée: {Notification}", 
                    JsonSerializer.Serialize(notification, new JsonSerializerOptions { WriteIndented = true }));

                // TODO: Implémenter l'envoi d'email ou notification Slack/Teams
                // await SendEmailNotificationAsync(notification);
                // await SendSlackNotificationAsync(notification);
                
                return Task.CompletedTask;
            }
            catch (Exception notificationEx)
            {
                _logger.LogError(notificationEx, "Erreur lors de l'envoi de notification admin");
                return Task.CompletedTask;
            }
        }

        public string GenerateErrorId()
        {
            return $"ERR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}[..8]";
        }

        public async Task SaveErrorDetailsAsync(string errorId, Exception exception, HttpContext? context = null)
        {
            try
            {
                var errorDetails = new
                {
                    ErrorId = errorId,
                    Timestamp = DateTime.UtcNow,
                    ExceptionType = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.ToString(),
                    Context = context != null ? new
                    {
                        Path = context.Request.Path.ToString(),
                        Method = context.Request.Method,
                        QueryString = context.Request.QueryString.ToString(),
                        Headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                        User = context.User?.Identity?.Name,
                        IPAddress = context.Connection.RemoteIpAddress?.ToString()
                    } : null,
                    Environment = _environment.EnvironmentName,
                    MachineName = Environment.MachineName
                };

                // Sauvegarder dans un fichier de log structuré
                var logPath = Path.Combine(_environment.ContentRootPath, "Logs", "Errors");
                Directory.CreateDirectory(logPath);
                
                var fileName = $"error-{DateTime.UtcNow:yyyyMMdd}.json";
                var filePath = Path.Combine(logPath, fileName);
                
                var json = JsonSerializer.Serialize(errorDetails, new JsonSerializerOptions { WriteIndented = true });
                await File.AppendAllTextAsync(filePath, json + Environment.NewLine);
            }
            catch (Exception saveEx)
            {
                _logger.LogError(saveEx, "Erreur lors de la sauvegarde des détails d'erreur");
            }
        }

        private bool IsCriticalError(Exception exception)
        {
            return exception switch
            {
                SqlException => true,
                Microsoft.EntityFrameworkCore.DbUpdateException => true,
                OutOfMemoryException => true,
                StackOverflowException => true,
                _ => false
            };
        }

        private string GetSeverityLevel(Exception exception)
        {
            return exception switch
            {
                ArgumentException => "Warning",
                UnauthorizedAccessException => "Warning",
                KeyNotFoundException => "Warning",
                SqlException => "Critical",
                Microsoft.EntityFrameworkCore.DbUpdateException => "Critical",
                OutOfMemoryException => "Critical",
                StackOverflowException => "Critical",
                _ => "Error"
            };
        }
    }
}