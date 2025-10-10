using Administration.Models;
using Administration.Models.Dao;
using Administration.Models.Entities;
using Administration.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Administration.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventDao _eventDao;
        private readonly UserDao _userDao;
        private readonly IAuthenticationService _authService;

        public EventsController(EventDao eventDao, UserDao userDao, IAuthenticationService authService)
        {
            _eventDao = eventDao;
            _userDao = userDao;
            _authService = authService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _eventDao.SelectAllAsync();
                return View(events);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la récupération des événements.";
                return View(new List<Event>());
            }
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var eventEntity = await _eventDao.SelectAsync(id);
                if (eventEntity == null)
                {
                    TempData["ErrorMessage"] = "Événement non trouvé.";
                    return RedirectToAction(nameof(Index));
                }
                return View(eventEntity);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la récupération de l'événement.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var currentUsername = _authService.GetCurrentUsername();
                if (string.IsNullOrEmpty(currentUsername))
                {
                    TempData["ErrorMessage"] = "Vous devez être connecté pour créer un événement.";
                    return RedirectToAction("Login", "Auth");
                }

                var newEvent = new Event 
                { 
                    DateTime = DateTime.Now.AddDays(1),
                    MaxParticipants = 10
                };

                var currentUser = await GetCurrentUserAsync();
                ViewBag.CurrentUser = currentUser;
                ViewBag.CurrentUsername = currentUsername;

                return View(newEvent);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors du chargement de la page.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventEntity)
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "Utilisateur connecté non trouvé. Veuillez vous reconnecter.";
                    return RedirectToAction("Login", "Auth");
                }

                eventEntity.UserId = currentUser.Id;
                
                ModelState.Remove("UserId");

                if (ModelState.IsValid)
                {
                    await _eventDao.InputAsync(eventEntity);
                    TempData["SuccessMessage"] = $"Événement '{eventEntity.Title}' créé avec succès par {currentUser.FullName}.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.CurrentUser = currentUser;
                ViewBag.CurrentUsername = currentUser.FullName;
                return View(eventEntity);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la création de l'événement.";
                
                return View(eventEntity);
            }
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var eventEntity = await _eventDao.SelectAsync(id);
                if (eventEntity == null)
                {
                    TempData["ErrorMessage"] = "Événement non trouvé.";
                    return RedirectToAction(nameof(Index));
                }
                return View(eventEntity);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors du chargement de l'événement.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventEntity)
        {
            if (id != eventEntity.Id)
            {
                TempData["ErrorMessage"] = "Données incohérentes.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "Vous devez être connecté pour modifier un événement.";
                    return RedirectToAction("Login", "Auth");
                }

                ModelState.Remove("UserId");
                ModelState.Remove("User");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    TempData["ErrorMessage"] = $"Données invalides : {string.Join(", ", errors)}";
                    ViewBag.CurrentUser = currentUser;
                    ViewBag.CurrentUsername = currentUser.FullName;
                    return View(eventEntity);
                }

                var success = await _eventDao.PutAsync(eventEntity);
                if (success)
                {
                    TempData["SuccessMessage"] = $"Événement '{eventEntity.Title}' modifié avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors de la sauvegarde en base de données. Vérifiez les logs.";
                    ViewBag.CurrentUser = currentUser;
                    ViewBag.CurrentUsername = currentUser.FullName;
                    return View(eventEntity);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception lors de la modification : {ex.Message}";
                
                var currentUser = await GetCurrentUserAsync();
                ViewBag.CurrentUser = currentUser;
                ViewBag.CurrentUsername = currentUser?.FullName ?? _authService.GetCurrentUsername();
                
                return View(eventEntity);
            }
        }

        // POST: Events/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var eventEntity = await _eventDao.SelectAsync(id);
                
                if (eventEntity == null)
                {
                    TempData["ErrorMessage"] = "Événement non trouvé.";
                    return RedirectToAction(nameof(Index));
                }

                var success = await _eventDao.DeleteAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = $"Événement '{eventEntity.Title}' supprimé avec succès.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors de la suppression de l'événement.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression de l'événement.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();
                
                if (user == null)
                {
                    var email = _authService.GetCurrentUserEmail();
                    
                    if (!string.IsNullOrEmpty(email))
                    {
                        var users = await _userDao.SelectAllAsync();
                        user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                    }
                }
                
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}