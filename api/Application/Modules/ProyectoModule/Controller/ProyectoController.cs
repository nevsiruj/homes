using Application.Modules.ProyectoModule.Services;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.ProyectoModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoService _ProyectoService;

        public ProyectoController(IProyectoService ProyectoService) => _ProyectoService = ProyectoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proyecto>>> GetAllProyectos()
        {
            var proyectos = await _ProyectoService.GetAllAsync();
            return Ok(proyectos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proyecto>> GetProyecto(int id)
        {
            var proyecto = await _ProyectoService.GetByIdAsync(id);
            return proyecto == null ? NotFound() : Ok(proyecto);
        }


        //Para slug
        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<Proyecto>> GetProyectoBySlug([FromRoute] string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest("El slug no puede estar vacío.");

            // (Opcional) normalizar/decodificar por si viene URL-encoded o con mayúsculas
            slug = Uri.UnescapeDataString(slug).Trim();

            var proyecto = await _ProyectoService.GetBySlugAsync(slug);
            return proyecto is null ? NotFound() : Ok(proyecto);
        }

        [HttpPost]
        public async Task<ActionResult<Proyecto>> CreateProyecto(ProyectoDTO data)
        {
            var creado = await _ProyectoService.CreateAsync(data);
            return CreatedAtAction(nameof(GetProyecto), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProyecto(int id, ProyectoDTO data)
        {
            if (id != data.Id)
            {
                return BadRequest();
            }

            await _ProyectoService.UpdateAsync(data);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ProyectoService.DeleteAsync(id);
            return NoContent();
        }
    }
}