using Administration.Models.Dao;
using Administration.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsApiController : ControllerBase
    {
        private readonly ProgramDao _programDao;

        public ProgramsApiController(ProgramDao programDao)
        {
            _programDao = programDao;
        }

        // GET: api/ProgramsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramEntity>>> GetPrograms()
        {
            var programs = await _programDao.SelectAllAsync();
            return Ok(programs);
        }

        // GET: api/ProgramsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgramEntity>> GetProgram(int id)
        {
            var program = await _programDao.SelectAsync(id);
            if (program == null)
            {
                return NotFound();
            }
            return Ok(program);
        }

        // POST: api/ProgramsApi
        [HttpPost]
        public async Task<ActionResult<ProgramEntity>> CreateProgram(ProgramEntity program)
        {
            try
            {
                var createdProgram = await _programDao.InputAsync(program);
                return CreatedAtAction(nameof(GetProgram), new { id = createdProgram.Id }, createdProgram);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/ProgramsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, ProgramEntity program)
        {
            if (id != program.Id)
            {
                return BadRequest();
            }

            var success = await _programDao.PutAsync(program);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/ProgramsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var success = await _programDao.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/ProgramsApi/difficulty/beginner
        [HttpGet("difficulty/{difficulty}")]
        public async Task<ActionResult<IEnumerable<ProgramEntity>>> GetProgramsByDifficulty(string difficulty)
        {
            var programs = await _programDao.GetProgramsByDifficultyAsync(difficulty);
            return Ok(programs);
        }

        // GET: api/ProgramsApi/price?min=10&max=100
        [HttpGet("price")]
        public async Task<ActionResult<IEnumerable<ProgramEntity>>> GetProgramsByPriceRange([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var programs = await _programDao.GetProgramsByPriceRangeAsync(min, max);
            return Ok(programs);
        }

        // POST: api/ProgramsApi/5/trainings/3
        [HttpPost("{programId}/trainings/{trainingId}")]
        public async Task<IActionResult> AddTrainingToProgram(int programId, int trainingId)
        {
            var success = await _programDao.AddTrainingToProgramAsync(programId, trainingId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/ProgramsApi/5/trainings/3
        [HttpDelete("{programId}/trainings/{trainingId}")]
        public async Task<IActionResult> RemoveTrainingFromProgram(int programId, int trainingId)
        {
            var success = await _programDao.RemoveTrainingFromProgramAsync(programId, trainingId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}