using Administration.Data;
using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Models.Dao
{
    public class TestimonialDao
    {
        private readonly AdministrationDbContext _context;

        public TestimonialDao(AdministrationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Testimonial>> SelectAllAsync()
        {
            return await _context.Testimonials
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<Testimonial?> SelectAsync(int id)
        {
            return await _context.Testimonials
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Testimonial> InputAsync(Testimonial testimonial)
        {
            testimonial.CreatedDate = DateTime.Now;
            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();
            return testimonial;
        }

        public async Task<bool> PutAsync(Testimonial testimonial)
        {
            try
            {
                _context.Entry(testimonial).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var testimonial = await _context.Testimonials.FindAsync(id);
                if (testimonial != null)
                {
                    _context.Testimonials.Remove(testimonial);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Testimonial>> GetTestimonialsByRatingAsync(int rating)
        {
            return await _context.Testimonials
                .Where(t => t.Rating == rating)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Testimonial>> GetRecentTestimonialsAsync(int count = 10)
        {
            return await _context.Testimonials
                .OrderByDescending(t => t.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Testimonial>> GetFeaturedTestimonialsAsync()
        {
            return await _context.Testimonials
                .Where(t => t.Rating >= 4)
                .OrderByDescending(t => t.CreatedDate)
                .Take(5)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync()
        {
            var ratings = await _context.Testimonials
                .Where(t => t.Rating.HasValue)
                .Select(t => t.Rating!.Value)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }
    }
}