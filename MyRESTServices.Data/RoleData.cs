using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data
{
    public class RoleData : IRoleData
    {
        private readonly AppDbContext _context;

        public RoleData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                var role = await _context.Roles.FindAsync(roleId);

                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                if (role == null)
                {
                    throw new ArgumentException("Role not found");
                }

                role.Usernames.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add user to role", ex);
            }

            return Task.CompletedTask;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role != null)
                {
                    _context.Roles.Remove(role);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete role", ex);
            }
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.OrderBy(r => r.RoleName).ToListAsync();
        }

        public async Task<Role> GetById(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> Insert(Role entity)
        {
            try
            {
                _context.Roles.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert role", ex);
            }
        }

        public async Task<Role> Update(int id, Role entity)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role != null)
                {
                    role.RoleName = entity.RoleName;
                    _context.Entry(role).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return role;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update role", ex);
            }
        }
    }
}
