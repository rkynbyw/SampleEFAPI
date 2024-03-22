using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Helpers;
using MyRESTServices.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly AppSettings _appSettings;

        public UsersController(IUserBLL userBLL, IOptions<AppSettings> appSettings)
        {
            _userBLL = userBLL;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userBLL.GetAll();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userBLL.GetUserWithRoles(username);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateDTO userCreateDTO)
        {
            try
            {
                await _userBLL.Insert(userCreateDTO);
                return Ok("User added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add user: {ex.Message}");
            }
        }



        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginDTO loginDTO)
        //{
        //    try
        //    {

        //        var user = await _userBLL.Login(loginDTO.Username, loginDTO.Password);

        //        var userLogin = await _userBLL.GetUserWithRoles(user.Username);

        //        var roles = userLogin.Roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)).ToList();


        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name, user.Username),
        //            }),
        //            Expires = DateTime.UtcNow.AddHours(1),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
        //                SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        var token = tokenHandler.CreateToken(tokenDescriptor);
        //        var tokenString = tokenHandler.WriteToken(token);

        //        var userWithToken = new UserWithToken
        //        {
        //            Username = user.Username,
        //            Token = tokenString
        //        };

        //        return Ok(userWithToken);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userBLL.Login(loginDTO.Username, loginDTO.Password);

                var userLogin = await _userBLL.GetUserWithRoles(user.Username);

                var roleClaims = userLogin.Roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName));

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Username),
                    }.Union(roleClaims)), // Menggabungkan klaim peran (roles) dengan klaim nama pengguna
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var userWithToken = new UserWithToken
                {
                    Username = user.Username,
                    Roles = userLogin.Roles.Select(r => r.RoleName).ToList(), // Menambahkan informasi peran (roles) pengguna
                    Token = tokenString
                };

                return Ok(userWithToken);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("{username}/changepassword")]
        public async Task<IActionResult> ChangePassword(string username, string newpassword)
        {
            try
            {
                await _userBLL.ChangePassword(username, newpassword);
                return Ok("Password changed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
