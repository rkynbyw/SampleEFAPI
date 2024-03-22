using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserData _userData;
        private readonly IMapper _mapper;

        public UserBLL(IUserData userData, IMapper mapper)
        {
            _userData = userData ?? throw new ArgumentNullException(nameof(userData));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            await _userData.ChangePassword(username, newPassword);
            return Task.CompletedTask;
        }

        public async Task<Task> Delete(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _userData.GetAllWithRoles();
            var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDto;
        }

        public async Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            var users = await _userData.GetAllWithRoles();
            var usersDto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDto;
        }

        public async Task<UserDTO> GetByUsername(string username)
        {
            var user = await _userData.GetByUsername(username);
            if (user == null)
            {
                throw new ArgumentException($"User with username '{username}' not found.");
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }

        public async Task<UserDTO> GetUserWithRoles(string username)
        {
            var user = await _userData.GetUserWithRoles(username);
            if (user == null)
            {
                throw new ArgumentException($"User with username '{username}' not found.");
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }

        public async Task<Task> Insert(UserCreateDTO entity)
        {
            var user = _mapper.Map<User>(entity);
            await _userData.Insert(user);
            return Task.CompletedTask;
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var user = await _userData.Login(username, password);
            if (user == null)
            {
                throw new ArgumentException($"Invalid username or password.");
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }
    }
}
