using Administration.Models.Dao;
using Administration.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsApiController : ControllerBase
    {
        private readonly EventDao _eventDao;

        public EventsApiController(EventDao eventDao)
        {
            _eventDao = eventDao;
        }

        // GET: api/EventsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            try
            {
                var events = await _eventDao.SelectAllAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // GET: api/EventsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            try
            {
                var eventEntity = await _eventDao.SelectAsync(id);
                
                if (eventEntity == null)
                {
                    return NotFound(new { message = $"Événement avec l'ID {id} non trouvé" });
                }
                
                return Ok(eventEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // POST: api/EventsApi
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event eventEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdEvent = await _eventDao.InputAsync(eventEntity);
                
                return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // PUT: api/EventsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event eventEntity)
        {
            try
            {
                if (id != eventEntity.Id)
                {
                    return BadRequest(new { message = "L'ID dans l'URL ne correspond pas à l'ID de l'événement" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _eventDao.PutAsync(eventEntity);
                
                if (!success)
                {
                    return NotFound(new { message = $"Événement avec l'ID {id} non trouvé" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // DELETE: api/EventsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var success = await _eventDao.DeleteAsync(id);
                
                if (!success)
                {
                    return NotFound(new { message = $"Événement avec l'ID {id} non trouvé" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // GET: api/EventsApi/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByUser(int userId)
        {
            try
            {
                var events = await _eventDao.GetEventsByUserAsync(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // GET: api/EventsApi/upcoming
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<Event>>> GetUpcomingEvents()
        {
            try
            {
                var events = await _eventDao.GetUpcomingEventsAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}