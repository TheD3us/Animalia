using Administration.Data;
using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Models.Dao
{
    public class EventDao
    {
        private readonly AdministrationDbContext _context;

        public EventDao(AdministrationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> SelectAllAsync()
        {
            return await _context.Events
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<Event?> SelectAsync(int id)
        {
            return await _context.Events
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> InputAsync(Event eventEntity)
        {
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();
            return eventEntity;
        }

        public async Task<bool> PutAsync(Event eventEntity)
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            Directory.CreateDirectory(logPath);
            var logFile = Path.Combine(logPath, $"events-{DateTime.Now:yyyy-MM-dd}.log");
            
           
            
            try
            {
                
                var existingEvent = await _context.Events.FindAsync(eventEntity.Id);
                    
                if (existingEvent == null)
                {
                    return false;
                }

                existingEvent.Title = eventEntity.Title;
                existingEvent.DateTime = eventEntity.DateTime;
                existingEvent.Location = eventEntity.Location;
                existingEvent.Notes = eventEntity.Notes;
                existingEvent.MaxParticipants = eventEntity.MaxParticipants;
                
                var changes = await _context.SaveChangesAsync();
                
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return false;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var eventEntity = await _context.Events.FindAsync(id);
                if (eventEntity != null)
                {
                    _context.Events.Remove(eventEntity);
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

        public async Task<List<Event>> GetEventsByUserAsync(int userId)
        {
            return await _context.Events
                .Include(e => e.User)
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Event>> GetUpcomingEventsAsync()
        {
            return await _context.Events
                .Include(e => e.User)
                .Where(e => e.DateTime > DateTime.Now)
                .OrderBy(e => e.DateTime)
                .ToListAsync();
        }
    }
}