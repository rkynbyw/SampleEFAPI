using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.BLL
{
    public class RoleBLL : IRoleBLL
    {
        private readonly IRoleData _roleData;
        private readonly IMapper _mapper;

        public RoleBLL(IRoleData roleData, IMapper mapper)
        {
            _roleData = roleData ?? throw new ArgumentNullException(nameof(roleData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Task> AddRole(string roleName)
        {
            try
            {
                var role = new Role { RoleName = roleName };
                await _roleData.Insert(role);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add role", ex);
            }
        }

        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            try
            {
                await _roleData.AddUserToRole(username, roleId);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add user to role", ex);
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            try
            {
                var roles = await _roleData.GetAll();
                var rolesDto = _mapper.Map<IEnumerable<RoleDTO>>(roles);
                return rolesDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve roles", ex);
            }
        }
    }
}
