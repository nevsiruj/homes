using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Modules.InmuebleModule.Services
{
    public interface IInmuebleService : IGenericService<InmuebleDTO>
    {
        Task<PagedResult<InmuebleDTO>> GetAllPagedAsync(PaginationParamsDTO paginationParams);
        Task<Inmueble> GetBySlugAsync(string slugInmueble);
        Task UpdatePrecioActivoAsync(int id, bool isPrecioActivo);
    }

    internal static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    public class InmuebleService : GenericService<Inmueble, InmuebleDTO>, IInmuebleService
    {
        private readonly IGenericRepository<Inmueble> _repository;
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _context;

        public InmuebleService(IGenericRepository<Inmueble> repository, IMapper mapper, ProjectDbContext context)
            : base(repository, mapper, context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public override async Task<InmuebleDTO> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id,
                includes: new List<string> { "ImagenesReferencia", "Amenidades" });
            return _mapper.Map<InmuebleDTO>(entity);
        }

        public override async Task<IEnumerable<InmuebleDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync(
                includes: new List<string> { "ImagenesReferencia", "Amenidades" });
            return _mapper.Map<IEnumerable<InmuebleDTO>>(entities);
        }

        public override async Task<InmuebleDTO> CreateAsync(InmuebleDTO dto)
        {
            var entity = _mapper.Map<Inmueble>(dto);
            
            // Auto-generate slug if not provided
            if (string.IsNullOrWhiteSpace(entity.SlugInmueble))
            {
                entity.SlugInmueble = SlugGenerator.GenerateSlug(entity.Titulo, entity.CodigoPropiedad);
            }
            
            await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<InmuebleDTO>(entity);
        }

        public override async Task UpdateAsync(InmuebleDTO dto)
        {
            var inmuebleExistente = await _context.Set<Inmueble>()
                                                    .Include(i => i.Amenidades)
                                                    .Include(i => i.ImagenesReferencia)
                                                    .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (inmuebleExistente == null)
                throw new KeyNotFoundException($"Inmueble con ID {dto.Id} no encontrado.");

            _mapper.Map(dto, inmuebleExistente);

            // Auto-generate slug if not provided or empty
            if (string.IsNullOrWhiteSpace(inmuebleExistente.SlugInmueble))
            {
                inmuebleExistente.SlugInmueble = SlugGenerator.GenerateSlug(inmuebleExistente.Titulo, inmuebleExistente.CodigoPropiedad);
            }

            // Amenidades
            if (dto.Amenidades != null)
            {
                var newAmenidadIds = dto.Amenidades.Select(a => a.AmenidadInmuebleId).ToList();
                var toRemove = inmuebleExistente.Amenidades
                                                .Where(ai => !newAmenidadIds.Contains(ai.AmenidadInmuebleId))
                                                .ToList();
                foreach (var a in toRemove) inmuebleExistente.Amenidades.Remove(a);

                foreach (var aDto in dto.Amenidades)
                {
                    if (!inmuebleExistente.Amenidades.Any(ai => ai.AmenidadInmuebleId == aDto.AmenidadInmuebleId))
                    {
                        var amenidadEntity = await _context.Set<Domain.Entities.Amenidad>().FirstOrDefaultAsync(a => a.Id == aDto.AmenidadInmuebleId);
                        if (amenidadEntity != null)
                        {
                            inmuebleExistente.Amenidades.Add(new AmenidadInmueble
                            {
                                AmenidadInmuebleId = amenidadEntity.Id,
                                InmuebleId = inmuebleExistente.Id,
                                Nombre = amenidadEntity.Nombre
                            });
                        }
                    }
                }
            }
            else
            {
                inmuebleExistente.Amenidades.Clear();
            }

            // Imágenes
            if (dto.ImagenesReferencia != null)
            {
                var newUrls = dto.ImagenesReferencia.Select(img => img.Url).ToList();
                var toRemove = inmuebleExistente.ImagenesReferencia
                                                .Where(img => !newUrls.Contains(img.Url))
                                                .ToList();
                foreach (var img in toRemove) _context.Entry(img).State = EntityState.Deleted;

                foreach (var imgDto in dto.ImagenesReferencia)
                {
                    if (!inmuebleExistente.ImagenesReferencia.Any(img => img.Url == imgDto.Url))
                    {
                        inmuebleExistente.ImagenesReferencia.Add(new ImagenInmueble
                        {
                            Url = imgDto.Url,
                            InmuebleId = inmuebleExistente.Id
                        });
                    }
                }
            }
            else
            {
                inmuebleExistente.ImagenesReferencia.Clear();
            }

            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(object id)
        {
            var inmueble = await _context.Set<Inmueble>().FindAsync(id);
            if (inmueble == null)
                throw new KeyNotFoundException($"Inmueble con ID {id} no encontrado para eliminar.");
            _context.Set<Inmueble>().Remove(inmueble);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<InmuebleDTO>> GetAllPagedAsync(PaginationParamsDTO paginationParams)
        {
            const string ciAi = "Latin1_General_CI_AI";

            var query = _context.Set<Inmueble>()
                .Include(i => i.ImagenesReferencia)
                .Include(i => i.Amenidades)
                .AsQueryable();

            var predicate = PredicateBuilder.True<Inmueble>();

            // SearchTerm
            if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
            {
                var raw = paginationParams.SearchTerm.Trim();

                // Dividir el término de búsqueda en palabras clave
                var words = Regex
                    .Split(raw, @"[^\p{L}\p{N}]+") // Divide por espacios o caracteres no alfanuméricos
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .Select(w => w.ToLower())
                    .Where(w => w.Length >= 2) // Ignorar palabras muy cortas
                    .Distinct()
                    .ToList();

                // Crear un predicado para buscar coincidencias parciales de todas las palabras clave
                var searchPredicate = PredicateBuilder.True<Inmueble>();
                foreach (var word in words)
                {
                    var pattern = $"%{word}%";
                    searchPredicate = searchPredicate.And(i =>
                        EF.Functions.Like(EF.Functions.Collate(i.Titulo ?? "", ciAi), pattern) ||
                        EF.Functions.Like(EF.Functions.Collate(i.CodigoPropiedad ?? "", ciAi), pattern) ||
                        EF.Functions.Like(EF.Functions.Collate(i.Ubicaciones ?? "", ciAi), pattern) ||
                        EF.Functions.Like(EF.Functions.Collate(i.Tipos ?? "", ciAi), pattern)
                    );
                }

                // Combinar el predicado con el resto de los filtros
                predicate = predicate.And(searchPredicate);
            }

            // Operaciones
            if (!string.IsNullOrEmpty(paginationParams.Operaciones))
            {
                predicate = predicate.And(i => EF.Functions.Collate(i.Operaciones ?? "", ciAi) == paginationParams.Operaciones.Trim());
            }

            // Tipos
            if (paginationParams.Tipos != null && paginationParams.Tipos.Any())
            {
                var tiposPredicate = PredicateBuilder.False<Inmueble>();
                foreach (var tipo in paginationParams.Tipos)
                {
                    tiposPredicate = tiposPredicate.Or(i => EF.Functions.Collate(i.Tipos ?? "", ciAi) == tipo.Trim());
                }
                predicate = predicate.And(tiposPredicate);
            }

            // Ubicaciones
            if (paginationParams.Ubicaciones != null && paginationParams.Ubicaciones.Any())
            {
                var ubicacionesPredicate = PredicateBuilder.False<Inmueble>();
                foreach (var ubicacion in paginationParams.Ubicaciones)
                {
                    ubicacionesPredicate = ubicacionesPredicate.Or(i => EF.Functions.Collate(i.Ubicaciones ?? "", ciAi) == ubicacion.Trim());
                }
                predicate = predicate.And(ubicacionesPredicate);
            }

            // Habitaciones exactas
            if (paginationParams.Habitaciones != null && paginationParams.Habitaciones.Any())
            {
                var habitacionesPredicate = PredicateBuilder.False<Inmueble>();
                foreach (var hab in paginationParams.Habitaciones)
                {
                    habitacionesPredicate = habitacionesPredicate.Or(i => i.Habitaciones == hab);
                }
                predicate = predicate.And(habitacionesPredicate);
            }

            // Habitaciones mínimas
            if (paginationParams.HabitacionesMin != null && paginationParams.HabitacionesMin.Any())
            {
                var minReq = paginationParams.HabitacionesMin.Max();
                predicate = predicate.And(i => i.Habitaciones >= minReq);
            }

            // Amenidades
            if (paginationParams.Amenidades != null && paginationParams.Amenidades.Any())
            {
                var list = paginationParams.Amenidades.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()).ToList();
                if (list.Count > 0)
                {
                    var amenidadPredicate = PredicateBuilder.True<Inmueble>();
                    foreach (var amenidadNombre in list)
                    {
                        var tempAmenidadNombre = amenidadNombre;
                        amenidadPredicate = amenidadPredicate.And(i => i.Amenidades.Any(ai => EF.Functions.Collate(ai.Nombre ?? "", ciAi) == tempAmenidadNombre));
                    }
                    predicate = predicate.And(amenidadPredicate);
                }
            }

            // Precio mínimo
            if (paginationParams.PrecioMinimo.HasValue)
                predicate = predicate.And(i => i.Precio >= paginationParams.PrecioMinimo.Value);

            // Luxury
            if (paginationParams.Luxury == true)
                predicate = predicate.And(i => i.Luxury == true);

            // Precio máximo
            if (paginationParams.PrecioMaximo.HasValue)
                predicate = predicate.And(i => i.Precio <= paginationParams.PrecioMaximo.Value);

            // Filtro por título (separando palabras)
            if (!string.IsNullOrWhiteSpace(paginationParams.Titulo))
            {
                var raw = paginationParams.Titulo.Trim();
                var words = Regex
                    .Split(raw, @"[^\p{L}\p{N}]+")
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .Select(w => w.ToLower())
                    .Where(w => w.Length >= 2)
                    .Distinct()
                    .ToList();

                if (words.Count > 0)
                {
                    var titlePredicate = PredicateBuilder.False<Inmueble>();
                    foreach (var w in words)
                    {
                        var w1 = $"%{w}%";
                        titlePredicate = titlePredicate.Or(i =>
                            EF.Functions.Like(EF.Functions.Collate(i.Titulo ?? "", ciAi), w1) ||
                            EF.Functions.Like(EF.Functions.Collate(i.Contenido ?? "", ciAi), w1)
                        );
                    }
                    predicate = predicate.And(titlePredicate);
                }
            }

            // CodigoPropiedad
            if (!string.IsNullOrEmpty(paginationParams.CodigoPropiedad))
            {
                predicate = predicate.And(i => EF.Functions.Collate(i.CodigoPropiedad ?? "", ciAi) == paginationParams.CodigoPropiedad.Trim());
            }

            // Aplicar el predicado a la consulta
            query = query.Where(predicate);

            // Ordenamiento
            if (!string.IsNullOrEmpty(paginationParams.Orden))
            {
                switch (paginationParams.Orden.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(i => i.Precio);
                        break;
                    case "desc":
                        query = query.OrderByDescending(i => i.Precio);
                        break;
                    case "habitaciones_asc":
                        query = query.OrderBy(i => i.Habitaciones)
                                     .ThenBy(i => i.Precio)
                                     .ThenBy(i => i.Id);
                        break;
                    case "habitaciones_desc":
                        query = query.OrderByDescending(i => i.Habitaciones)
                                     .ThenByDescending(i => i.Precio)
                                     .ThenBy(i => i.Id);
                        break;
                    default:
                        query = query.OrderBy(i => i.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(i => i.Precio);
            }

            // Paginación
            var totalCount = await query.CountAsync();
            var entities = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<InmuebleDTO>>(entities);
            return new PagedResult<InmuebleDTO>(dtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<Inmueble> GetBySlugAsync(string slugInmueble)
        {
            if (string.IsNullOrWhiteSpace(slugInmueble)) return null;
            var s = Uri.UnescapeDataString(slugInmueble).Trim();

            return await _context.Set<Inmueble>()
                .Include(i => i.ImagenesReferencia)
                .Include(i => i.Amenidades)
                .FirstOrDefaultAsync(i =>
                    EF.Functions.Collate(i.SlugInmueble ?? "", "Latin1_General_CI_AI") == s);
        }

        public async Task UpdatePrecioActivoAsync(int id, bool isPrecioActivo)
        {
            var inmueble = await _context.Set<Inmueble>().FindAsync(id);
            if (inmueble == null)
                throw new KeyNotFoundException($"Inmueble con ID {id} no encontrado.");
            inmueble.PrecioActivo = isPrecioActivo;
            await _context.SaveChangesAsync();
        }
    }
}