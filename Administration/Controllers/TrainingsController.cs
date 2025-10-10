using Administration.Models.Dao;
using Administration.Models.Entities;
using Administration.Models.ViewModels;
using Administration.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Administration.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly TrainingDao _trainingDao;
        private readonly UserDao _userDao;
        private readonly ProgramDao _programDao;
        private readonly IAuthenticationService _authService;

        public TrainingsController(TrainingDao trainingDao, UserDao userDao, ProgramDao programDao, IAuthenticationService authService)
        {
            _trainingDao = trainingDao;
            _userDao = userDao;
            _programDao = programDao;
            _authService = authService;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            try
            {
                var trainings = await _trainingDao.SelectAllAsync();
                ViewBag.CurrentUsername = _authService.GetCurrentUsername();
                return View(trainings);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erreur lors du chargement des entrainements : {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["ErrorMessage"] += $" | Détails: {ex.InnerException.Message}";
                }
                return View(new List<Training>());
            }
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var training = await _trainingDao.SelectByIdAsync(id.Value);
                if (training == null)
                {
                    return NotFound();
                }

                return View(training);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors du chargement de l'entrainement.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Trainings/Create
        public async Task<IActionResult> Create()
        {
            await PopulateProgramsList();
            return View(new TrainingViewModel());
        }

        // POST: Trainings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUserAsync();
                    if (currentUser == null)
                    {
                        TempData["ErrorMessage"] = "Utilisateur connecté non trouvé. Veuillez vous reconnecter.";
                        return RedirectToAction("Login", "Auth");
                    }

                    var training = new Training
                    {
                        Title = viewModel.Title,
                        DurationMinutes = viewModel.DurationMinutes,
                        Equipment = viewModel.Equipment,
                        Level = viewModel.Level,
                        Description = viewModel.Description,
                        UserId = currentUser.Id
                    };

                    await _trainingDao.InsertAsync(training);

                    if (viewModel.SelectedProgramIds != null && viewModel.SelectedProgramIds.Any())
                    {
                        foreach (var programId in viewModel.SelectedProgramIds)
                        {
                            await _programDao.AddTrainingToProgramAsync(programId, training.Id);
                        }
                    }

                    TempData["SuccessMessage"] = "Entrainement créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur lors de la création de l'entrainement.");
                }
            }
            
            await PopulateProgramsList(viewModel.SelectedProgramIds);
            return View(viewModel);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var training = await _trainingDao.SelectByIdAsync(id.Value);
                if (training == null)
                {
                    return NotFound();
                }

                var viewModel = new TrainingViewModel
                {
                    Id = training.Id,
                    Title = training.Title,
                    DurationMinutes = training.DurationMinutes,
                    Equipment = training.Equipment,
                    Level = training.Level,
                    Description = training.Description,
                    SelectedProgramIds = await GetProgramsForTraining(training.Id)
                };

                await PopulateProgramsList(viewModel.SelectedProgramIds);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors du chargement de l'entrainement.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Trainings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTraining = await _trainingDao.SelectByIdAsync(id);
                    if (existingTraining == null)
                    {
                        TempData["ErrorMessage"] = "Entrainement non trouvé.";
                        return RedirectToAction(nameof(Index));
                    }

                    var training = new Training
                    {
                        Id = viewModel.Id,
                        Title = viewModel.Title,
                        DurationMinutes = viewModel.DurationMinutes,
                        Equipment = viewModel.Equipment,
                        Level = viewModel.Level,
                        Description = viewModel.Description,
                        UserId = existingTraining.UserId
                    };

                    await _trainingDao.UpdateAsync(training);
                    await UpdateTrainingPrograms(training.Id, viewModel.SelectedProgramIds ?? new List<int>());

                    TempData["SuccessMessage"] = "Entrainement modifié avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erreur lors de la modification de l'entrainement.");
                }
            }
            
            await PopulateProgramsList(viewModel.SelectedProgramIds);
            return View(viewModel);
        }

        // POST: Trainings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _trainingDao.DeleteAsync(id);
                TempData["SuccessMessage"] = "Entrainement supprimé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression de l'entrainement.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Méthodes utilitaires privées
        private async Task PopulateProgramsList(List<int>? selectedProgramIds = null)
        {
            var programs = await _programDao.SelectAllAsync();
            ViewBag.AvailablePrograms = programs.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Title} ({p.Difficulty} - {p.Price:C})",
                Selected = selectedProgramIds?.Contains(p.Id) ?? false
            }).ToList();
        }

        private async Task<List<int>> GetProgramsForTraining(int trainingId)
        {
            var programs = await _programDao.SelectAllAsync();
            return programs.Where(p => p.Trainings?.Any(t => t.Id == trainingId) ?? false)
                          .Select(p => p.Id)
                          .ToList();
        }

        private async Task UpdateTrainingPrograms(int trainingId, List<int> selectedProgramIds)
        {
            var currentProgramIds = await GetProgramsForTraining(trainingId);

            var programsToRemoveFrom = currentProgramIds.Except(selectedProgramIds).ToList();
            foreach (var programId in programsToRemoveFrom)
            {
                await _programDao.RemoveTrainingFromProgramAsync(programId, trainingId);
            }

            var programsToAddTo = selectedProgramIds.Except(currentProgramIds).ToList();
            foreach (var programId in programsToAddTo)
            {
                await _programDao.AddTrainingToProgramAsync(programId, trainingId);
            }
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            try
            {
                var currentUsername = _authService.GetCurrentUsername();
                if (string.IsNullOrEmpty(currentUsername))
                {
                    return null;
                }

                var users = await _userDao.SelectAllAsync();
                var adminUser = users.FirstOrDefault();
                return adminUser;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}