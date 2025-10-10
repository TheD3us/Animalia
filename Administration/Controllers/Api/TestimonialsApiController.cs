using Administration.Models.Dao;
using Administration.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Administration.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestimonialsApiController : ControllerBase
    {
        private readonly TestimonialDao _testimonialDao;

        public TestimonialsApiController(TestimonialDao testimonialDao)
        {
            _testimonialDao = testimonialDao;
        }

        // GET: api/TestimonialsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Testimonial>>> GetTestimonials()
        {
            var testimonials = await _testimonialDao.SelectAllAsync();
            return Ok(testimonials);
        }

        // GET: api/TestimonialsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Testimonial>> GetTestimonial(int id)
        {
            var testimonial = await _testimonialDao.SelectAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return Ok(testimonial);
        }

        // POST: api/TestimonialsApi
        [HttpPost]
        public async Task<ActionResult<Testimonial>> CreateTestimonial(Testimonial testimonial)
        {
            try
            {
                var createdTestimonial = await _testimonialDao.InputAsync(testimonial);
                return CreatedAtAction(nameof(GetTestimonial), new { id = createdTestimonial.Id }, createdTestimonial);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/TestimonialsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestimonial(int id, Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return BadRequest();
            }

            var success = await _testimonialDao.PutAsync(testimonial);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/TestimonialsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var success = await _testimonialDao.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/TestimonialsApi/rating/5
        [HttpGet("rating/{rating}")]
        public async Task<ActionResult<IEnumerable<Testimonial>>> GetTestimonialsByRating(int rating)
        {
            var testimonials = await _testimonialDao.GetTestimonialsByRatingAsync(rating);
            return Ok(testimonials);
        }

        // GET: api/TestimonialsApi/recent?count=5
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Testimonial>>> GetRecentTestimonials([FromQuery] int count = 10)
        {
            var testimonials = await _testimonialDao.GetRecentTestimonialsAsync(count);
            return Ok(testimonials);
        }

        // GET: api/TestimonialsApi/featured
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<Testimonial>>> GetFeaturedTestimonials()
        {
            var testimonials = await _testimonialDao.GetFeaturedTestimonialsAsync();
            return Ok(testimonials);
        }

        // GET: api/TestimonialsApi/average-rating
        [HttpGet("average-rating")]
        public async Task<ActionResult<object>> GetAverageRating()
        {
            var averageRating = await _testimonialDao.GetAverageRatingAsync();
            return Ok(new { averageRating = Math.Round(averageRating, 2) });
        }
    }
}