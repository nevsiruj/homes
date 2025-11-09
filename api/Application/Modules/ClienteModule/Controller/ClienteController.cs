using Application.Modules.ClienteModule.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Application.Modules.ClienteModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpGet("dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var cliente = await _clienteService.GetClienteByDni(dni);
            return cliente == null ? NotFound() : Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteDTO data)
        {
            var created = await _clienteService.CreateAsync(data);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDto data)
        {
            if (id != data.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            try
            {
                var updated = await _clienteService.UpdateAsync(data); // ahora UpdateAsync devuelve ClienteDto
                return Ok(updated); // 👈 devolvemos el cliente actualizado
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 50,
            [FromQuery] string? agenteId = null)
        {
            try
            {

                var authHeader = Request.Headers["Authorization"].ToString();
                if (!authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized();
                }

                // Validar parámetros
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 50;
                if (pageSize > 2000) pageSize = 2000; // Límite máximo aumentado para cargar más clientes

                var clientes = await _clienteService.GetAllAsync();

                if (clientes == null || !clientes.Any())
                {
                    return Ok(new 
                    { 
                        data = new List<ClienteDto>(),
                        page = page,
                        pageSize = pageSize,
                        totalCount = 0,
                        totalPages = 0
                    });
                }

                // Filtrar por agente si se proporciona
                if (!string.IsNullOrEmpty(agenteId))
                {
                    clientes = clientes.Where(c => c.AgenteId == agenteId).ToList();
                }

                // Aplicar paginación
                var totalCount = clientes.Count();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                
                var pagedClientes = clientes
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new 
                { 
                    data = pagedClientes,
                    page = page,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = totalPages
                });
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al intentar traer todos los clientes: {ex.Message}");
                return StatusCode(500, $"¡Algo salió mal en el servidor! Error: {ex.Message}");
            }
        }

        [HttpGet("telefono/{telefono}")]
        public async Task<IActionResult> GetByTelefono(string telefono)
        {
            var cliente = await _clienteService.GetByTelefonoAsync(telefono);

            if (cliente == null)
            {
                return NotFound(new { message = "El teléfono no está registrado. Puedes crear un nuevo cliente." });
            }

            return Ok(cliente);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateClienteStatusDto data)
        {
            try
            {
                await _clienteService.UpdateStatusAsync(id, data.Status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado: {ex.Message}");
            }
        }

    }
}