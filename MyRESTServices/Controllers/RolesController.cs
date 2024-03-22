using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleBLL _roleBLL;

        public RolesController(IRoleBLL roleBLL)
        {
            _roleBLL = roleBLL ?? throw new ArgumentNullException(nameof(roleBLL));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAll()
        {
            try
            {
                var roles = await _roleBLL.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve roles: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            try
            {
                await _roleBLL.AddRole(roleName);
                return Ok("Role added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add role: {ex.Message}");
            }
        }

        [HttpPost("AddUserToRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddUserToRole(string username, int roleId)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            try
            {
                await _roleBLL.AddUserToRole(username, roleId);
                return Ok("User added to role successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add user to role: {ex.Message}");
            }
        }
    }
}
