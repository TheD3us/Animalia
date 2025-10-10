using Administration.Models.Dao;
using Administration.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly UserDao _userDao;

        public UsersApiController(UserDao userDao)
        {
            _userDao = userDao;
        }

        // GET: api/UsersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userDao.SelectAllAsync();
            return Ok(users);
        }

        // GET: api/UsersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userDao.SelectAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/UsersApi
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            try
            {
                // Vérifier si l'email existe déjà
                if (await _userDao.EmailExistsAsync(user.Email))
                {
                    return BadRequest(new { message = "Un utilisateur avec cet email existe déjà." });
                }

                var createdUser = await _userDao.InputAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/UsersApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            // Vérifier si l'email existe déjà pour un autre utilisateur
            if (await _userDao.EmailExistsAsync(user.Email, user.Id))
            {
                return BadRequest(new { message = "Un autre utilisateur avec cet email existe déjà." });
            }

            var success = await _userDao.PutAsync(user);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/UsersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userDao.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/UsersApi/email/john@example.com
        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _userDao.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/UsersApi/search?name=john
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsersByName([FromQuery] string name)
        {
            var users = await _userDao.SearchUsersByNameAsync(name);
            return Ok(users);
        }
    }
}