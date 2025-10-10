using Administration.Data;
using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Models.Dao
{
    public class ProgramDao
    {
        private readonly AdministrationDbContext _context;

        public ProgramDao(AdministrationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProgramEntity>> SelectAllAsync()
        {
            try
            {
                return await _context.Programs
                    .Include(p => p.Trainings)
                    .OrderBy(p => p.Title)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erreur lors de la récupération des programmes", ex);
            }
        }

        public async Task<ProgramEntity?> SelectAsync(int id)
        {
            try
            {
                return await _context.Programs
                    .Include(p => p.Trainings)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la récupération du programme {id}", ex);
            }
        }

        public async Task<ProgramEntity> InputAsync(ProgramEntity program)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task<bool> PutAsync(ProgramEntity program)
        {
            try
            {
                _context.Entry(program).State = EntityState.Modified;
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
                var program = await _context.Programs.FindAsync(id);
                if (program != null)
                {
                    _context.Programs.Remove(program);
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

        public async Task<List<ProgramEntity>> GetProgramsByDifficultyAsync(string difficulty)
        {
            return await _context.Programs
                .Include(p => p.Trainings)
                .Where(p => p.Difficulty == difficulty)
                .ToListAsync();
        }

        public async Task<List<ProgramEntity>> GetProgramsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Programs
                .Include(p => p.Trainings)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<bool> AddTrainingToProgramAsync(int programId, int trainingId)
        {
            try
            {
                var program = await _context.Programs.Include(p => p.Trainings).FirstOrDefaultAsync(p => p.Id == programId);
                var training = await _context.Trainings.FindAsync(trainingId);

                if (program != null && training != null)
                {
                    program.Trainings.Add(training);
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

        public async Task<bool> RemoveTrainingFromProgramAsync(int programId, int trainingId)
        {
            try
            {
                var program = await _context.Programs.Include(p => p.Trainings).FirstOrDefaultAsync(p => p.Id == programId);
                var training = program?.Trainings.FirstOrDefault(t => t.Id == trainingId);

                if (program != null && training != null)
                {
                    program.Trainings.Remove(training);
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

        public async Task<List<Training>> GetAvailableTrainingsAsync()
        {
            return await _context.Trainings
                .OrderBy(t => t.Title)
                .ToListAsync();
        }
    }
}