using Administration.Services;
using Administration.Data;
using Administration.Models.Dao;
using Administration.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container - Configuration MVC
builder.Services.AddControllersWithViews(options =>
{
    // Ajouter le filtre d'authentification global
    options.Filters.Add<AuthenticationFilter>();
});

// Configuration des sessions pour l'authentification
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "Administration.Session";
});

// Services d'authentification
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Configuration Entity Framework pour utiliser la base Animalia existante
builder.Services.AddDbContext<AdministrationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' n'est pas configurée.");
    }

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(60);
    });
    
    // Enable detailed logging in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});

// Register DAOs pour l'accès aux données
builder.Services.AddScoped<EventDao>();
builder.Services.AddScoped<ProgramDao>();
builder.Services.AddScoped<TrainingDao>();
builder.Services.AddScoped<UserDao>();
builder.Services.AddScoped<TestimonialDao>();

// Register service de gestion d'erreurs
builder.Services.AddScoped<IErrorService, ErrorService>();

// Configure HttpClient et service API pour l'API externe Animalia
builder.Services.AddHttpClient<IAnimaliaApiService, AnimaliaApiService>();
builder.Services.AddScoped<IAnimaliaApiService, AnimaliaApiService>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
