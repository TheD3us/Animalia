using Administration.Data;
using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Models.Dao
{
    public class TrainingDao
    {
        private readonly AdministrationDbContext _context;

        public TrainingDao(AdministrationDbContext context)
        {
            _context = context;
        }

       
        public async Task<List<Training>> SelectAllAsync()
        {
            return await _context.Trainings
                .OrderBy(t => t.Title)
                .ToListAsync();
        }

        public async Task<Training?> SelectByIdAsync(int id)
        {
            return await _context.Trainings
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Training> InsertAsync(Training training)
        {
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();
            return training;
        }

        public async Task<Training> UpdateAsync(Training training)
        {
            _context.Entry(training).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return training;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            if (training == null)
            {
                return false;
            }

            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Training>> SelectByLevelAsync(string level)
        {
            return await _context.Trainings
                .Where(t => t.Level == level)
                .OrderBy(t => t.Title)
                .ToListAsync();
        }

        public async Task<List<Training>> SelectByMinDurationAsync(int minDuration)
        {
            return await _context.Trainings
                .Where(t => t.DurationMinutes >= minDuration)
                .OrderBy(t => t.Title)
                .ToListAsync();
        }
    }
}