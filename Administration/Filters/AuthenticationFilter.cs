using Administration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Administration.Filters
{
    /// <summary>
    /// Filtre global d'authentification pour l'interface d'administration.
    /// Vérifie que l'utilisateur est authentifié et possède les droits admin.
    /// </summary>
    public class AuthenticationFilter : IActionFilter
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationFilter(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// S'exécute AVANT chaque action de contrôleur.
        /// Vérifie l'authentification et les droits admin.
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Vérifier si l'action permet l'accès anonyme (attribut [AllowAnonymous])
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (allowAnonymous)
            {
                return; // Pas d'authentification requise
            }

            // 2. Récupérer le nom du contrôleur et de l'action
            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];

            // 3. Pages publiques autorisées sans authentification
            if (controllerName == "Auth" || 
                (controllerName == "Home" && actionName == "Error"))
            {
                return;
            }

            // 4. Vérifier si l'utilisateur est authentifié
            if (!_authService.IsAuthenticated())
            {
                // Rediriger vers la page de connexion avec URL de retour
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl });
                return;
            }

            // 5. Vérifier si l'utilisateur a les droits d'administration
            if (!_authService.IsAdmin())
            {
                // Rediriger vers la page d'accès refusé
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }

            // Si toutes les vérifications passent, l'action du contrôleur s'exécutera normalement
        }

        /// <summary>
        /// S'exécute APRÈS chaque action de contrôleur.
        /// Aucune action post-traitement nécessaire pour ce filtre.
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Rien à faire après l'exécution de l'action
        }
    }
}