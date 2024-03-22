using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Helpers;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class UserData : IUserData
    {
        private readonly AppDbContext _context;

        public UserData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                user.Password = Md5Hash.GetHash(newPassword);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found");
            }
            return Task.CompletedTask;
        }


        public async Task<bool> Delete(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.OrderBy(u => u.Username).ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            // Tidak perlu implementasi untuk GetById, biarkan kosong
            return null;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllWithRoles()
        {
            return await _context.Users.Include(u => u.Roles).OrderBy(u => u.Username).ToListAsync();
        }

        public async Task<User> GetUserWithRoles(string username)
        {
            return await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> Insert(User entity)
        {
            try
            {
                entity.Password = Md5Hash.GetHash(entity.Password);
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert user", ex);
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var hashedPassword = Md5Hash.GetHash(password);
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
        }

        public async Task<User> Update(string username, User entity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                user.Username = entity.Username;
                user.Password = entity.Password;
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Telp = entity.Telp;

                await _context.SaveChangesAsync();
                return user;
            }
            else
            {
                throw new ArgumentException("User not found");
            }
        }

        public Task<User> Update(int id, User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
