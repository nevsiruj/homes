using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Modules.ZonaModule.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Application.Modules.ZonaModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZonaController : ControllerBase
    {
        private readonly IZonaService _zonaService;

        public ZonaController(IZonaService zonaService)
        {
            _zonaService = zonaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var zonas = await _zonaService.GetAllByCustomOrderAsync();
                return Ok(new
                {
                    success = true,
                    data = zonas,
                    total = zonas.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener zonas",
                    error = ex.Message
                });
            }
        }

        [HttpGet("activas")]
        public async Task<IActionResult> GetAllActivas()
        {
            try
            {
                var zonas = await _zonaService.GetAllActivasByCustomOrderAsync();
                return Ok(new
                {
                    success = true,
                    data = zonas,
                    total = zonas.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener zonas activas",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var zona = await _zonaService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    data = zona
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener la zona",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateZonaDto createZonaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var zonaDto = new ZonaDto
                {
                    Nombre = createZonaDto.Nombre,
                    Activo = createZonaDto.Activo ?? true,
                    Orden = createZonaDto.Orden ?? 0
                };

                var zona = await _zonaService.CreateAsync(zonaDto);
                return CreatedAtAction(nameof(GetById), new { id = zona.Id }, new
                {
                    success = true,
                    data = zona,
                    message = "Zona creada exitosamente"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al crear la zona",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ZonaDto zonaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != zonaDto.Id)
                    return BadRequest(new
                    {
                        success = false,
                        message = "El ID de la ruta no coincide con el ID del cuerpo."
                    });

                await _zonaService.UpdateAsync(zonaDto);
                return Ok(new
                {
                    success = true,
                    message = "Zona actualizada exitosamente"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al actualizar la zona",
                    error = ex.Message
                });
            }
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<int> zonaIds)
        {
            try
            {
                if (zonaIds == null || zonaIds.Count == 0)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Debe proporcionar una lista de IDs de zonas para reordenar."
                    });

                await _zonaService.ReorderZonasAsync(zonaIds);
                return Ok(new
                {
                    success = true,
                    message = "Zonas reordenadas exitosamente"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al reordenar zonas",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _zonaService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Zona eliminada exitosamente"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al eliminar la zona",
                    error = ex.Message
                });
            }
        }
    }
}
