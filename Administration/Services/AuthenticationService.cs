using Administration.Models.Dao;
using Administration.Models.Entities;

namespace Administration.Services
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<bool> ValidateAdminCredentialsAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        void SignIn(string email, int userId);
        void SignOut();
        bool IsAuthenticated();
        bool IsAdmin();
        string? GetCurrentUserEmail();
        int? GetCurrentUserId();
        string? GetCurrentUsername();
        Task<User?> GetCurrentUserAsync();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly UserDao _userDao;

        private const string SESSION_KEY = "IsAuthenticated";
        private const string EMAIL_KEY = "UserEmail";
        private const string USER_ID_KEY = "UserId";
        private const string IS_ADMIN_KEY = "IsAdmin";

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, ILogger<AuthenticationService> logger, UserDao userDao)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _userDao = userDao;
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                // Récupérer l'utilisateur par email
                var user = await GetUserByEmailAsync(email);
                
                if (user == null)
                {
                    return false;
                }

                // Vérifier le mot de passe (version simple - mot de passe en clair)
                var isValid = user.Password == password;

                return isValid;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ValidateAdminCredentialsAsync(string email, string password)
        {
            try
            {
                var credentialsValid = await ValidateCredentialsAsync(email, password);
                
                if (!credentialsValid)
                {
                    return false;
                }

                var user = await GetUserByEmailAsync(email);
                
                if (user == null)
                {
                    return false;
                }

                if (!user.IsAdmin)
                {
                    return false;
                }

         

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                var users = await _userDao.SelectAllAsync();
                return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SignIn(string email, int userId)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                // Récupérer l'utilisateur pour stocker le statut admin en session
                var user = GetUserByEmailAsync(email).GetAwaiter().GetResult();
                
                session.SetString(SESSION_KEY, "true");
                session.SetString(EMAIL_KEY, email);
                session.SetInt32(USER_ID_KEY, userId);
                session.SetString(IS_ADMIN_KEY, user?.IsAdmin.ToString() ?? "false");
            }
        }

        public void SignOut()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            
            if (session != null)
            {
                session.Remove(SESSION_KEY);
                session.Remove(EMAIL_KEY);
                session.Remove(USER_ID_KEY);
                session.Remove(IS_ADMIN_KEY);
                session.Clear();
            }
        }

        public bool IsAuthenticated()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString(SESSION_KEY) == "true";
        }

        public bool IsAdmin()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var isAdminStr = session?.GetString(IS_ADMIN_KEY);
            return bool.TryParse(isAdminStr, out var isAdmin) && isAdmin;
        }

        public string? GetCurrentUserEmail()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString(EMAIL_KEY);
        }

        public int? GetCurrentUserId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetInt32(USER_ID_KEY);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(email))
                return null;

            return await GetUserByEmailAsync(email);
        }

        // Méthode de compatibilité pour l'ancien système
        public string? GetCurrentUsername()
        {
            var email = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(email))
                return null;

            // Pour la compatibilité, retourner juste "admin" 
            // Les vues utiliseront ViewBag.CurrentUsername pour le nom complet
            if (email == "admin@animalia.com")
                return "admin";
            
            return email.Split('@')[0];
        }

        // Méthodes de compatibilité avec l'ancienne interface
        public bool ValidateCredentials(string username, string password)
        {
            // Utiliser la validation admin pour l'interface d'administration
            return ValidateAdminCredentialsAsync(username, password).GetAwaiter().GetResult();
        }

        public void SignIn(string username)
        {
            // Pour la compatibilité, essayer de récupérer l'utilisateur
            var user = GetUserByEmailAsync(username).GetAwaiter().GetResult();
            if (user != null)
            {
                SignIn(user.Email, user.Id);
            }
        }
    }
}