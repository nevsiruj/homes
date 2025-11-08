using Application.Modules.MatchModule.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Modules.MatchModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatchDto data)
        {
            var created = await _matchService.CreateAsync(data);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            var match = await _matchService.GetByIdAsync(id);
            return match == null ? NotFound() : Ok(match);
        }

        //[HttpGet("cliente/{clienteId}/pendientes")]
        //public async Task<IActionResult> GetPendientesByCliente(int clienteId)
        //{

        //    var matches = await _matchService.GetPendientesByClienteIdAsync(clienteId);

        //    if (matches == null || !matches.Any())
        //    {
        //        return NotFound($"No se encontraron matches pendientes para el cliente ID: {clienteId}");
        //    }

        //    return Ok(matches);
        //}

        [HttpGet("cliente/{clienteId}/enviados")]
        public async Task<IActionResult> GetEnviadosByCliente(int clienteId)
        {
            var matches = await _matchService.GetEnviadosByClienteIdAsync(clienteId);
            return Ok(matches);
        }

        [HttpPut("{id}/enviar")]
        public async Task<IActionResult> MarcarComoEnviado(int id)
        {
            try
            {
                await _matchService.MarcarComoEnviadoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al marcar el match como enviado: {ex.Message}");
            }
        }

       

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                
                await _matchService.DeleteAsync(id);

               
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                
                return NotFound($"El Match con ID {id} no existe.");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Error al eliminar el Match: {ex.Message}");
            }
        }


        [HttpGet("clientes/{clienteId}/matches-sugeridos")]
        public async Task<IActionResult> GetMatchesSugeridos(int clienteId)
        {
            try
            {
                var matches = await _matchService.GenerarMatchesSugeridos(clienteId);
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}