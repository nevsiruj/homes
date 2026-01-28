using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Application.Modules.UsersModule.Services;

namespace Application.Modules.UsersModule.Controllers
{
    [Route("[controller]")]
    //[Authorize] // Agregar el atributo [Authorize] para proteger todo el controlador
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
        }

      

        [HttpGet("ClientsCount")]
        [Authorize]
        public async Task<IActionResult> ClientsCount()
        {
            try
            {
                int clientCount = await _userService.GetClientCount();
                return Ok(new { cantidad = clientCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserWithImageAsync(model);

            if (result.Succeeded)
            {

                return Ok(new { message = "Usuario creado exitosamente." });
            }
            else
            {

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
        }



        // Obtener todos los usuarios
        [HttpGet]
        //[Authorize]
        public async Task<List<UserDTO>> GetUsersWithRoles()
        {
            var usersWithRoles = new List<UserDTO>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                UserDTO newUser = _mapper.Map<UserDTO>(user);

               

                newUser.Rol = userRoles.Any() ? userRoles[0] : "Rol no asignado";
                usersWithRoles.Add(newUser);
            }


            return usersWithRoles;
        }



        // Obtener un usuario por ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
      
        [HttpGet("Admin")]
        [Authorize]
        public async Task<ActionResult> GetAdmin()
        {
            var user = await _userService.GetUsersAdmin();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("Agente")]
        [Authorize]
        public async Task<ActionResult> GetAgente()
        {
            var user = await _userService.GetUsersAgente();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        // Editar un usuario existente
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDTO updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Asegurarse de usar el id de la ruta como fuente de verdad
                updatedUser.Id = id;

                var result = await _userService.Update(updatedUser);

                if (result)
                {
                    return Ok(new { ok = true });
                }

                // Si Update devolvió false, comprobar si el usuario existe para devolver el código correcto
                var userExists = await _userManager.FindByIdAsync(id);
                if (userExists == null)
                {
                    return NotFound();
                }

                return BadRequest(new { message = "No se pudo actualizar el usuario." });
            }
            catch (KeyNotFoundException knfException)
            {
                return NotFound(knfException.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Un error ocurrió mientras se procesaba su solicitud.");
            }
        }

        // PATCH: actualiza solamente la contraseña (New + ConfirmNew)
        [HttpPatch("{id}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
            {
                return BadRequest(new { message = "NewPassword and ConfirmNewPassword are required." });
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest(new { message = "NewPassword and ConfirmNewPassword do not match." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userService.ChangePasswordAsync(id, request.NewPassword);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }



        // Eliminar un usuario
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}
