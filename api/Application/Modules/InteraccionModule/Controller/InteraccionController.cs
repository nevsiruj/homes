using Application.Modules.InteraccionModule.Service;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.InteraccionModule.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class InteraccionController : ControllerBase
    {
        private readonly IInteraccionService _service;

        public InteraccionController(IInteraccionService service)
        {
            _service = service;
        }
    

    [HttpGet]
        public async Task<ActionResult<IEnumerable<InteraccionDTO>>> Get()
        {
            var lista = await _service.ObtenerTodasAsync();
            var resultado = lista.Select(i => new InteraccionDTO
            {
                Id = i.Id,
                Fecha = i.Fecha,
                FechaVencimiento = i.FechaVencimiento,
                Tipo = i.Tipo,
                Descripcion = i.Descripcion,
                AgenteId = i.AgenteId,
                ClienteId =i.ClienteId,
                StatusInteraccion = i.StatusInteraccion
            });
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InteraccionDTO>> GetById(int id)
        {
            var interaccion = await _service.ObtenerPorIdAsync(id);
            if (interaccion == null) return NotFound();

            return new InteraccionDTO
            {
                Id = interaccion.Id,
                Fecha = interaccion.Fecha,
                FechaVencimiento = interaccion.FechaVencimiento,
                Tipo = interaccion.Tipo,
                Descripcion = interaccion.Descripcion,
                AgenteId = interaccion.AgenteId,
                ClienteId = interaccion.ClienteId,
                StatusInteraccion = interaccion.StatusInteraccion
            };
        }

        [HttpPost]
        public async Task<ActionResult<InteraccionDTO>> Post([FromBody] InteraccionDTO dto)
        {
            var interaccion = new Interaccion
            {
                Fecha = dto.Fecha,
                FechaVencimiento = dto.FechaVencimiento,
                Tipo = dto.Tipo,
                Descripcion = dto.Descripcion,
                AgenteId = dto.AgenteId,
                ClienteId = dto.ClienteId,
                StatusInteraccion = dto.StatusInteraccion
            };

            await _service.CrearAsync(interaccion);

            return CreatedAtAction(nameof(GetById), new { id = interaccion.Id }, new InteraccionDTO
            {
                Id = interaccion.Id,
                Fecha = interaccion.Fecha,
                FechaVencimiento = interaccion.FechaVencimiento,
                Tipo = interaccion.Tipo,
                Descripcion = interaccion.Descripcion,
                AgenteId = interaccion.AgenteId,
                ClienteId = interaccion.ClienteId,
                StatusInteraccion = interaccion.StatusInteraccion
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] InteraccionDTO dto)
        {
            var interaccion = new Interaccion
            {   
                Id = id,
                Fecha = dto.Fecha,
                FechaVencimiento =dto.FechaVencimiento,
                Tipo = dto.Tipo,
                Descripcion = dto.Descripcion,
                AgenteId = dto.AgenteId,
                ClienteId = dto.ClienteId,
                StatusInteraccion = dto.StatusInteraccion
            };

            await _service.ActualizarAsync(interaccion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string nuevoStatus)
        {
            var interaccion = await _service.ObtenerPorIdAsync(id);
            if (interaccion == null) return NotFound();

            await _service.ActualizarStatusAsync(id, nuevoStatus); 
            return NoContent();
        }
    }
}
