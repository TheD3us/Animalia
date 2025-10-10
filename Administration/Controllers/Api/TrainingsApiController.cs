using Administration.Models.Dao;
using Administration.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingsApiController : ControllerBase
    {
        private readonly TrainingDao _trainingDao;

        public TrainingsApiController(TrainingDao trainingDao)
        {
            _trainingDao = trainingDao;
        }

        /// <summary>
        /// Récupère tous les entrainements
        /// </summary>
        /// <returns>Liste des entrainements</returns>
        // GET: api/TrainingsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainings()
        {
            try
            {
                var trainings = await _trainingDao.SelectAllAsync();
                return Ok(trainings);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la récupération des entrainements");
            }
        }

        /// <summary>
        /// Récupère un entrainement par son ID
        /// </summary>
        /// <param name="id">ID de l'entrainement</param>
        /// <returns>L'entrainement correspondant</returns>
        // GET: api/TrainingsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> GetTraining(int id)
        {
            try
            {
                var training = await _trainingDao.SelectByIdAsync(id);
                if (training == null)
                {
                    return NotFound($"Entrainement avec l'ID {id} non trouvé");
                }
                return Ok(training);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la récupération de l'entrainement");
            }
        }

        /// <summary>
        /// Crée un nouvel entrainement
        /// </summary>
        /// <param name="training">Données de l'entrainement</param>
        /// <returns>L'entrainement créé</returns>
        // POST: api/TrainingsApi
        [HttpPost]
        public async Task<ActionResult<Training>> PostTraining(Training training)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                training.CreatedAt = DateTime.Now;
                training.UpdatedAt = DateTime.Now;

                await _trainingDao.InsertAsync(training);
                return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, training);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la création de l'entrainement");
            }
        }

        /// <summary>
        /// Met à jour un entrainement existant
        /// </summary>
        /// <param name="id">ID de l'entrainement</param>
        /// <param name="training">Données mises à jour</param>
        /// <returns>Résultat de la mise à jour</returns>
        // PUT: api/TrainingsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(int id, Training training)
        {
            if (id != training.Id)
            {
                return BadRequest("L'ID dans l'URL ne correspond pas à l'ID de l'entrainement");
            }

            try
            {
                var existingTraining = await _trainingDao.SelectByIdAsync(id);
                if (existingTraining == null)
                {
                    return NotFound($"Entrainement avec l'ID {id} non trouvé");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                training.UpdatedAt = DateTime.Now;
                await _trainingDao.UpdateAsync(training);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la mise à jour de l'entrainement");
            }
        }

        /// <summary>
        /// Supprime un entrainement
        /// </summary>
        /// <param name="id">ID de l'entrainement à supprimer</param>
        /// <returns>Résultat de la suppression</returns>
        // DELETE: api/TrainingsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            try
            {
                var training = await _trainingDao.SelectByIdAsync(id);
                if (training == null)
                {
                    return NotFound($"Entrainement avec l'ID {id} non trouvé");
                }

                await _trainingDao.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la suppression de l'entrainement");
            }
        }

        /// <summary>
        /// Récupère les entrainements actifs uniquement
        /// </summary>
        /// <returns>Liste des entrainements actifs</returns>
        // GET: api/TrainingsApi/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Training>>> GetActiveTrainings()
        {
            try
            {
                var trainings = await _trainingDao.SelectAllAsync();
                var activeTrainings = trainings.Where(t => t.IsActive).ToList();
                return Ok(activeTrainings);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la récupération des entrainements actifs");
            }
        }

        /// <summary>
        /// Récupère les entrainements par type
        /// </summary>
        /// <param name="type">Type d'entrainement</param>
        /// <returns>Liste des entrainements du type spécifié</returns>
        // GET: api/TrainingsApi/by-type/{type}
        [HttpGet("by-type/{type}")]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsByType(string type)
        {
            try
            {
                var trainings = await _trainingDao.SelectAllAsync();
                var filteredTrainings = trainings.Where(t => 
                    t.TrainingType.Equals(type, StringComparison.OrdinalIgnoreCase) && t.IsActive
                ).ToList();
                
                return Ok(filteredTrainings);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur serveur lors de la récupération des entrainements par type");
            }
        }
    }
}