using Administration.Data;
using Administration.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Administration.Models.Dao
{
    public class UserDao
    {
        private readonly AdministrationDbContext _context;

        public UserDao(AdministrationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> SelectAllAsync()
        {
            return await _context.Users
                .Include(u => u.Events)
                .ToListAsync();
        }

        public async Task<User?> SelectAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> InputAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> PutAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
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
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> SearchUsersByNameAsync(string name)
        {
            return await _context.Users
                .Include(u => u.Events)
                .Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && (excludeUserId == null || u.Id != excludeUserId));
        }
    }
}