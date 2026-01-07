using AlmacenSystem.Infrastructure.Repositories.Common;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Modules.BlogModule.Services
{
    public interface IArticuloService : IGenericService<ArticuloDto>
    {
        /// <summary>
        /// Obtiene todos los artículos activos
        /// </summary>
        Task<IEnumerable<ArticuloDto>> GetAllActivosAsync();

        /// <summary>
        /// Obtiene artículos por categoría
        /// </summary>
        Task<IEnumerable<ArticuloDto>> GetByCategoriaAsync(string categoria);

        /// <summary>
        /// Obtiene un artículo por su slug
        /// </summary>
        Task<ArticuloDto> GetBySlugAsync(string slug);

        /// <summary>
        /// Obtiene artículos ordenados por orden personalizado
        /// </summary>
        Task<IEnumerable<ArticuloDto>> GetAllByCustomOrderAsync();

        /// <summary>
        /// Obtiene artículos activos ordenados por orden personalizado
        /// </summary>
        Task<IEnumerable<ArticuloDto>> GetAllActivosByCustomOrderAsync();

        /// <summary>
        /// Reordena los artículos según el orden proporcionado
        /// </summary>
        Task ReorderArticulosAsync(List<int> articuloIds);

        /// <summary>
        /// Busca artículos por título o contenido
        /// </summary>
        Task<IEnumerable<ArticuloDto>> SearchAsync(string termino);
    }

    public class ArticuloService : GenericService<Articulo, ArticuloDto>, IArticuloService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public ArticuloService(
            IGenericRepository<Articulo> repository,
            IMapper mapper,
            ProjectDbContext context)
            : base(repository, mapper, context)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task<ArticuloDto> GetByIdAsync(object id)
        {
            var articulo = await _context.Articulos.FindAsync((int)id);
            if (articulo == null)
                throw new KeyNotFoundException($"Artículo con ID {id} no encontrado.");
            return _mapper.Map<ArticuloDto>(articulo);
        }

        public override async Task<IEnumerable<ArticuloDto>> GetAllAsync()
        {
            var articulos = await _context.Articulos
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllByCustomOrderAsync()
        {
            var articulos = await _context.Articulos
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllActivosAsync()
        {
            var articulos = await _context.Articulos
                .Where(a => a.Activo)
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllActivosByCustomOrderAsync()
        {
            var articulos = await _context.Articulos
                .Where(a => a.Activo)
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public async Task<IEnumerable<ArticuloDto>> GetByCategoriaAsync(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("La categoría no puede estar vacía.");

            var articulos = await _context.Articulos
                .Where(a => a.Activo && a.Categoria.ToLower() == categoria.ToLower())
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public async Task<ArticuloDto> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("El slug no puede estar vacío.");

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(a => a.Slug.ToLower() == slug.ToLower());

            if (articulo == null)
                throw new KeyNotFoundException($"Artículo con slug '{slug}' no encontrado.");

            return _mapper.Map<ArticuloDto>(articulo);
        }

        public async Task ReorderArticulosAsync(List<int> articuloIds)
        {
            if (articuloIds == null || articuloIds.Count == 0)
                throw new ArgumentException("La lista de IDs de artículos no puede estar vacía.");

            var articulos = await _context.Articulos
                .Where(a => articuloIds.Contains(a.Id))
                .ToListAsync();

            if (articulos.Count != articuloIds.Count)
                throw new KeyNotFoundException("Uno o más artículos no existen.");

            for (int i = 0; i < articuloIds.Count; i++)
            {
                var articulo = articulos.FirstOrDefault(a => a.Id == articuloIds[i]);
                if (articulo != null)
                {
                    articulo.Orden = i + 1;
                }
            }

            _context.Articulos.UpdateRange(articulos);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticuloDto>> SearchAsync(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                throw new ArgumentException("El término de búsqueda no puede estar vacío.");

            var terminoLower = termino.ToLower();
            var articulos = await _context.Articulos
                .Where(a => a.Activo && (
                    a.Titulo.ToLower().Contains(terminoLower) ||
                    a.Contenido.ToLower().Contains(terminoLower) ||
                    a.Categoria.ToLower().Contains(terminoLower)
                ))
                .OrderBy(a => a.Orden)
                .ThenByDescending(a => a.FechaCreacion)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ArticuloDto>>(articulos);
        }

        public override async Task<ArticuloDto> CreateAsync(ArticuloDto articuloDto)
        {
            if (string.IsNullOrWhiteSpace(articuloDto.Titulo))
                throw new ArgumentException("El título del artículo es obligatorio.");

            if (string.IsNullOrWhiteSpace(articuloDto.Contenido))
                throw new ArgumentException("El contenido del artículo es obligatorio.");

            if (string.IsNullOrWhiteSpace(articuloDto.Slug))
                throw new ArgumentException("El slug del artículo es obligatorio.");

            // Validar que no exista un artículo con el mismo slug
            var existente = await _context.Articulos
                .FirstOrDefaultAsync(a => a.Slug.ToLower() == articuloDto.Slug.ToLower());

            if (existente != null)
                throw new InvalidOperationException($"Ya existe un artículo con el slug '{articuloDto.Slug}'.");

            // Generar permalink basado en categoria y slug
            var categoriaNormalizada = articuloDto.Categoria?.Trim().ToLower().Replace(" ", "-") ?? "sin-categoria";
            var permalink = $"https://homesguatemala.com/{categoriaNormalizada}/{articuloDto.Slug}/";

            int proximoOrden = articuloDto.Orden > 0 ? articuloDto.Orden : (await _context.Articulos.MaxAsync(a => (int?)a.Orden) ?? 0) + 1;

            var articulo = new Articulo
            {
                Titulo = articuloDto.Titulo.Trim(),
                Contenido = articuloDto.Contenido.Trim(),
                Slug = articuloDto.Slug.ToLower().Trim(),
                Permalink = permalink,
                ImagenUrl = articuloDto.ImagenUrl?.Trim(),
                Categoria = articuloDto.Categoria?.Trim() ?? "Sin categoría",
                Activo = articuloDto.Activo,
                Orden = proximoOrden,
                FechaCreacion = DateTime.UtcNow
            };

            await _context.Articulos.AddAsync(articulo);
            await _context.SaveChangesAsync();

            return _mapper.Map<ArticuloDto>(articulo);
        }

        public override async Task UpdateAsync(ArticuloDto articuloDto)
        {
            var articulo = await _context.Articulos.FindAsync(articuloDto.Id);
            if (articulo == null)
                throw new KeyNotFoundException($"Artículo con ID {articuloDto.Id} no encontrado.");

            if (string.IsNullOrWhiteSpace(articuloDto.Titulo))
                throw new ArgumentException("El título del artículo es obligatorio.");

            if (string.IsNullOrWhiteSpace(articuloDto.Contenido))
                throw new ArgumentException("El contenido del artículo es obligatorio.");

            if (string.IsNullOrWhiteSpace(articuloDto.Slug))
                throw new ArgumentException("El slug del artículo es obligatorio.");

            // Validar que no exista otro artículo con el mismo slug
            var existente = await _context.Articulos
                .FirstOrDefaultAsync(a => a.Id != articuloDto.Id && a.Slug.ToLower() == articuloDto.Slug.ToLower());

            if (existente != null)
                throw new InvalidOperationException($"Ya existe un artículo con el slug '{articuloDto.Slug}'.");

            articulo.Titulo = articuloDto.Titulo.Trim();
            articulo.Contenido = articuloDto.Contenido.Trim();
            articulo.Slug = articuloDto.Slug.ToLower().Trim();
            var categoriaNormalizada = articuloDto.Categoria?.Trim().ToLower().Replace(" ", "-") ?? "sin-categoria";
            articulo.Permalink = $"https://homesguatemala.com/{categoriaNormalizada}/{articuloDto.Slug.ToLower()}/";
            articulo.ImagenUrl = articuloDto.ImagenUrl?.Trim();
            articulo.Categoria = articuloDto.Categoria?.Trim() ?? "Sin categoría";
            articulo.Activo = articuloDto.Activo;
            articulo.Orden = articuloDto.Orden;
            articulo.FechaActualizacion = DateTime.UtcNow;

            _context.Articulos.Update(articulo);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(object id)
        {
            var articulo = await _context.Articulos.FindAsync((int)id);
            if (articulo == null)
                throw new KeyNotFoundException($"Artículo con ID {id} no encontrado.");

            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();
        }
    }
}