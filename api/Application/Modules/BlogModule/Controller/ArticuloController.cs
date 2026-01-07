using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Modules.BlogModule.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Application.Modules.BlogModule.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticuloController : ControllerBase
    {
        private readonly IArticuloService _articuloService;

        public ArticuloController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var articulos = await _articuloService.GetAllByCustomOrderAsync();
                return Ok(new
                {
                    success = true,
                    data = articulos,
                    total = articulos.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener artículos",
                    error = ex.Message
                });
            }
        }

       
        [HttpGet("activos")]
        public async Task<IActionResult> GetAllActivos()
        {
            try
            {
                var articulos = await _articuloService.GetAllActivosByCustomOrderAsync();
                return Ok(new
                {
                    success = true,
                    data = articulos,
                    total = articulos.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener artículos activos",
                    error = ex.Message
                });
            }
        }

      
        [HttpGet("categoria/{categoria}")]
        public async Task<IActionResult> GetByCategoria(string categoria)
        {
            try
            {
                var articulos = await _articuloService.GetByCategoriaAsync(categoria);
                return Ok(new
                {
                    success = true,
                    data = articulos,
                    total = articulos.Count()
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener artículos por categoría",
                    error = ex.Message
                });
            }
        }

    
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            try
            {
                var articulo = await _articuloService.GetBySlugAsync(slug);
                return Ok(new
                {
                    success = true,
                    data = articulo
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
                    message = "Error al obtener artículo por slug",
                    error = ex.Message
                });
            }
        }
     
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var articulo = await _articuloService.GetByIdAsync(id);
                return Ok(new
                {
                    success = true,
                    data = articulo
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
                    message = "Error al obtener el artículo",
                    error = ex.Message
                });
            }
        }

      
        [HttpGet("buscar/{termino}")]
        public async Task<IActionResult> Search(string termino)
        {
            try
            {
                var articulos = await _articuloService.SearchAsync(termino);
                return Ok(new
                {
                    success = true,
                    data = articulos,
                    total = articulos.Count()
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al buscar artículos",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticuloDto createArticuloDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var articuloDto = new ArticuloDto
                {
                    Titulo = createArticuloDto.Titulo,
                    Contenido = createArticuloDto.Contenido,
                    Slug = createArticuloDto.Slug,
                    ImagenUrl = createArticuloDto.ImagenUrl,
                    Categoria = createArticuloDto.Categoria,
                    Activo = createArticuloDto.Activo ?? true,
                    Orden = createArticuloDto.Orden ?? 0
                };

                var articulo = await _articuloService.CreateAsync(articuloDto);
                return CreatedAtAction(nameof(GetById), new { id = articulo.Id }, new
                {
                    success = true,
                    data = articulo,
                    message = "Artículo creado exitosamente"
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
                    message = "Error al crear el artículo",
                    error = ex.Message
                });
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArticuloDto articuloDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != articuloDto.Id)
                    return BadRequest(new
                    {
                        success = false,
                        message = "El ID de la ruta no coincide con el ID del cuerpo."
                    });

                await _articuloService.UpdateAsync(articuloDto);
                return Ok(new
                {
                    success = true,
                    message = "Artículo actualizado exitosamente"
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
                    message = "Error al actualizar el artículo",
                    error = ex.Message
                });
            }
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<int> articuloIds)
        {
            try
            {
                if (articuloIds == null || articuloIds.Count == 0)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Debe proporcionar una lista de IDs de artículos para reordenar."
                    });

                await _articuloService.ReorderArticulosAsync(articuloIds);
                return Ok(new
                {
                    success = true,
                    message = "Artículos reordenados exitosamente"
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
                    message = "Error al reordenar artículos",
                    error = ex.Message
                });
            }
        }

 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _articuloService.DeleteAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Artículo eliminado exitosamente"
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
                    message = "Error al eliminar el artículo",
                    error = ex.Message
                });
            }
        }
    }
}