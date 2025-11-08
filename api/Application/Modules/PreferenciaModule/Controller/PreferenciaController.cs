using Application.Modules.PreferenciaModule.Service;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Modules.PreferenciaModule.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PreferenciaController : ControllerBase
    {
        private readonly IPreferenciaService _preferenciaService;

        public PreferenciaController(IPreferenciaService preferenciaService)
        {
            _preferenciaService = preferenciaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            var preferencias = await _preferenciaService.GetByClienteIdAsync(clienteId);
            if (preferencias == null || preferencias.Count == 0)
            {
                return NotFound("No se encontraron preferencias para el cliente.");
            }
            return Ok(preferencias);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePreferenciaDTO nuevaPreferencia)
        {
            if (nuevaPreferencia.ClienteId <= 0)
                return BadRequest("ClienteId es requerido.");

            var result = await _preferenciaService.AddAsync(nuevaPreferencia);
            return Ok(result);
        }


        //[HttpPut("{preferenciaId}")]
        //public async Task<IActionResult> Update(int preferenciaId, [FromBody] PreferenciaDTO preferenciaDto)
        //{
        //    if (preferenciaId != preferenciaDto.Id)
        //    {
        //        return BadRequest("ID de preferencia no coincide.");
        //    }
        //    await _preferenciaService.UpdateAsync(preferenciaId, preferenciaDto);
        //    return NoContent();
        //}

        [HttpPut("{preferenciaId}")]
        public async Task<IActionResult> Update(int preferenciaId, [FromBody] PreferenciaDTO preferenciaDto)
        {
            // Asigna el ID de la URL al DTO para asegurar que siempre coincidan.
            preferenciaDto.Id = preferenciaId;

            // Llama al servicio, la validación anterior ya no es necesaria.
            await _preferenciaService.UpdateAsync(preferenciaId, preferenciaDto);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPreferencia(int id, [FromBody] PreferenciaDTO preferenciaDto)
        {
            if (preferenciaDto == null)
                return BadRequest("Los datos enviados son inválidos.");

            try
            {
                await _preferenciaService.PatchAsync(id, preferenciaDto);
                return NoContent(); // 204
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpDelete("{preferenciaId}")]
        public async Task<IActionResult> Delete(int preferenciaId)
        {
            await _preferenciaService.DeleteAsync(preferenciaId);
            return NoContent();
        }
    }
}