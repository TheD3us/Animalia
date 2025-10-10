using Administration.Models.Dao;
using Administration.Models.Entities;
using Administration.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Administration.Controllers
{
    public class ProgramsController : Controller
    {
        private readonly ProgramDao _programDao;
        private readonly TrainingDao _trainingDao;

        public ProgramsController(ProgramDao programDao, TrainingDao trainingDao)
        {
            _programDao = programDao;
            _trainingDao = trainingDao;
        }

        // GET: Programs
        public async Task<IActionResult> Index()
        {
            try
            {
                var programs = await _programDao.SelectAllAsync();
                return View(programs);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la récupération des programmes.";
                return View(new List<ProgramEntity>());
            }
        }

        // GET: Programs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var program = await _programDao.SelectAsync(id);
                if (program == null)
                {
                    TempData["ErrorMessage"] = "Programme non trouvé.";
                    return RedirectToAction(nameof(Index));
                }
                return View(program);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la récupération du programme.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Programs/Create
        public async Task<IActionResult> Create()
        {
            await PopulateTrainingsList();
            return View(new ProgramViewModel());
        }

        // POST: Programs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgramViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var program = new ProgramEntity
                    {
                        Title = viewModel.Title,
                        Summary = viewModel.Summary,
                        Difficulty = viewModel.Difficulty,
                        Price = viewModel.Price,
                        ImageUrl = viewModel.ImageUrl
                    };

                    await _programDao.InputAsync(program);

                    if (viewModel.SelectedTrainingIds != null && viewModel.SelectedTrainingIds.Any())
                    {
                        foreach (var trainingId in viewModel.SelectedTrainingIds)
                        {
                            await _programDao.AddTrainingToProgramAsync(program.Id, trainingId);
                        }
                    }

                    TempData["SuccessMessage"] = "Programme créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }

                await PopulateTrainingsList(viewModel.SelectedTrainingIds);
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la création du programme.";
                await PopulateTrainingsList(viewModel.SelectedTrainingIds);
                return View(viewModel);
            }
        }

        // GET: Programs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var program = await _programDao.SelectAsync(id);
                if (program == null)
                {
                    TempData["ErrorMessage"] = "Programme non trouvé.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new ProgramViewModel
                {
                    Id = program.Id,
                    Title = program.Title,
                    Summary = program.Summary,
                    Difficulty = program.Difficulty,
                    Price = program.Price,
                    ImageUrl = program.ImageUrl,
                    SelectedTrainingIds = program.Trainings?.Select(t => t.Id).ToList() ?? new List<int>()
                };

                await PopulateTrainingsList(viewModel.SelectedTrainingIds);
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors du chargement du programme.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Programs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProgramViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                TempData["ErrorMessage"] = "Données incohérentes.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var program = new ProgramEntity
                    {
                        Id = viewModel.Id,
                        Title = viewModel.Title,
                        Summary = viewModel.Summary,
                        Difficulty = viewModel.Difficulty,
                        Price = viewModel.Price,
                        ImageUrl = viewModel.ImageUrl
                    };

                    var success = await _programDao.PutAsync(program);
                    if (success)
                    {
                        await UpdateProgramTrainings(program.Id, viewModel.SelectedTrainingIds ?? new List<int>());
                        TempData["SuccessMessage"] = "Programme modifié avec succès.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Programme non trouvé ou erreur lors de la modification.";
                    }
                }

                await PopulateTrainingsList(viewModel.SelectedTrainingIds);
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la modification du programme.";
                await PopulateTrainingsList(viewModel.SelectedTrainingIds);
                return View(viewModel);
            }
        }

        // POST: Programs/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _programDao.DeleteAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Programme supprimé avec succès.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Programme non trouvé ou erreur lors de la suppression.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression du programme.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateTrainingsList(List<int>? selectedTrainingIds = null)
        {
            var trainings = await _trainingDao.SelectAllAsync();
            ViewBag.AvailableTrainings = trainings.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Title} ({t.DurationMinutes} min - {t.Level})",
                Selected = selectedTrainingIds?.Contains(t.Id) ?? false
            }).ToList();
        }

        private async Task UpdateProgramTrainings(int programId, List<int> selectedTrainingIds)
        {
            var program = await _programDao.SelectAsync(programId);
            if (program == null) return;

            var currentTrainingIds = program.Trainings?.Select(t => t.Id).ToList() ?? new List<int>();

            var trainingsToRemove = currentTrainingIds.Except(selectedTrainingIds).ToList();
            foreach (var trainingId in trainingsToRemove)
            {
                await _programDao.RemoveTrainingFromProgramAsync(programId, trainingId);
            }

            var trainingsToAdd = selectedTrainingIds.Except(currentTrainingIds).ToList();
            foreach (var trainingId in trainingsToAdd)
            {
                await _programDao.AddTrainingToProgramAsync(programId, trainingId);
            }
        }
    }
}