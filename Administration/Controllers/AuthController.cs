using Administration.Models;
using Administration.Services;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // GET: Auth/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginModel());
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var isValidAdmin = await _authService.ValidateAdminCredentialsAsync(model.Email, model.Password);

                if (isValidAdmin)
                {
                    var user = await _authService.GetUserByEmailAsync(model.Email);
                    if (user != null)
                    {
                        _authService.SignIn(user.Email, user.Id);
                        
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Email ou mot de passe incorrect, ou vous n'avez pas les droits d'administration.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Une erreur s'est produite lors de la connexion. Veuillez réessayer.");
            }

            return View(model);
        }

        // POST: Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                var userEmail = _authService.GetCurrentUserEmail();
                _authService.SignOut();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de la déconnexion.";
            }

            return RedirectToAction("Login");
        }

        // GET: Auth/AccessDenied
        public async Task<IActionResult> AccessDenied()
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                ViewBag.CurrentUserEmail = currentUser.Email;
                ViewBag.CurrentUserName = currentUser.FullName;
                ViewBag.IsAdmin = currentUser.IsAdmin;
            }
            
            return View();
        }
    }
}