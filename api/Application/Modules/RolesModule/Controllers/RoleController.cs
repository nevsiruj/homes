using Application.Modules.RolesModule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.ProductosModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleService _service;

        public RoleController(RoleManager<IdentityRole> roleManager, IRoleService service)
        {
            _roleManager = roleManager;
            _service = service;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetRoles()
        {
            var roles = _service.GetRoles(); // Implementa este método en tu servicio de autenticación para obtener los roles disponibles
            return Ok(roles);
        }


        [HttpPost("AddRole")]
        //[Authorize]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var existingRole = await _roleManager.FindByNameAsync(roleName);
            if (existingRole != null)
            {
                return Conflict($"Role '{roleName}' already exists.");
            }

            var newRole = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' created successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpDelete("DeleteRole")]
        //[Authorize]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound($"Role '{roleName}' not found.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' deleted successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

    }
}
