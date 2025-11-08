using Application.Modules.InmuebleModule.Services;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.InmuebleModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InmuebleController : ControllerBase
    {
        private readonly IInmuebleService _InmuebleService;

        public InmuebleController(IInmuebleService InmuebleService) => _InmuebleService = InmuebleService;


        [HttpGet]
        public async Task<ActionResult<PagedResult<InmuebleDTO>>> GetPagedInmuebles([FromQuery] PaginationParamsDTO paginationParams)
        {
            var pagedInmuebles = await _InmuebleService.GetAllPagedAsync(paginationParams);
            return Ok(pagedInmuebles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            var inmueble = await _InmuebleService.GetByIdAsync(id);
            return inmueble == null ? NotFound() : Ok(inmueble);
        }

        //Para formatear el slug 
        //[HttpGet("by-slug/{slug}")]
        //public async Task<ActionResult<Inmueble>> GetInmuebleBySlug(string slug)
        //{
        //    if (string.IsNullOrEmpty(slug))
        //    {
        //        return BadRequest("El slug no puede estar vacío.");
        //    }

        //    // Extraer el código de propiedad del final del slug
        //    var partesDelSlug = slug.Split('-');
        //    string codigoPropiedad = partesDelSlug.Last();

        //    // Llama a un nuevo método del servicio para buscar por el código de propiedad
        //    var inmueble = await _InmuebleService.GetByCodigoAsync(codigoPropiedad);

        //    return inmueble == null ? NotFound() : Ok(inmueble);
        //}
        [HttpGet("by-slug/{slug}")]
        public async Task<ActionResult<Inmueble>> GetInmuebleBySlug([FromRoute] string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest("El slug no puede estar vacío.");

            // (Opcional) normalizar/decodificar por si viene URL-encoded o con mayúsculas
            slug = Uri.UnescapeDataString(slug).Trim();

            var inmueble = await _InmuebleService.GetBySlugAsync(slug);
            return inmueble is null ? NotFound() : Ok(inmueble);
        }

        [HttpPost]
        public async Task<ActionResult<Inmueble>> CreateInmueble(InmuebleDTO data)
        {
            var creado = await _InmuebleService.CreateAsync(data);
            return CreatedAtAction(nameof(GetInmueble), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInmueble(int id, InmuebleDTO data)
        {
            if (id != data.Id)
            {
                return BadRequest();
            }

            await _InmuebleService.UpdateAsync(data);
            return NoContent();
        }

        [HttpPatch("{id}/precio-activo")]
        public async Task<IActionResult> UpdatePrecioActivo(int id, [FromBody] PrecioActivoUpdateDto dto)
        {
            try
            {
                await _InmuebleService.UpdatePrecioActivoAsync(id, dto.PrecioActivo);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _InmuebleService.DeleteAsync(id);
            return NoContent();
        }
    }
}