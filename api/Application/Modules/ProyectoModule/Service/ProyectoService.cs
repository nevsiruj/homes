using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.Entities;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Application.Modules.ProyectoModule.Services
{
    public interface IProyectoService : IGenericService<ProyectoDTO>
    {
        // buscar por el slug completo
        Task<Proyecto> GetBySlugAsync(string slugProyecto);
    }

    public class ProyectoService : GenericService<Proyecto, ProyectoDTO>, IProyectoService
    {
        private readonly IGenericRepository<Proyecto> _repository;
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _context; 


        public ProyectoService(IGenericRepository<Proyecto> repository, IMapper mapper, ProjectDbContext context)
            : base(repository, mapper, context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context; 
        }

        public override async Task<ProyectoDTO> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id,
                includes: new List<string> { "ImagenesReferenciaProyecto", "Amenidades" });

            return _mapper.Map<ProyectoDTO>(entity);
        }

        public override async Task<IEnumerable<ProyectoDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync(
                includes: new List<string> { "ImagenesReferenciaProyecto", "Amenidades" });

            return _mapper.Map<IEnumerable<ProyectoDTO>>(entities);
        }

        public override async Task<ProyectoDTO> CreateAsync(ProyectoDTO dto)
        {
            var entity = _mapper.Map<Proyecto>(dto);

            await _repository.CreateAsync(entity);
            await _repository.SaveChangesAsync();

            //if (entity.Amenidades != null)
            //{
            //    foreach (var amenidad in entity.Amenidades)
            //    {
            //        amenidad.ProyectoId = entity.Id;
            //    }
            //    await _repository.SaveChangesAsync();
            //}

            return _mapper.Map<ProyectoDTO>(entity);
        }

        //Agregado para poder actualizar las amenidades Proyectos
        //public override async Task UpdateAsync(ProyectoDTO dto)
        //{

        //    var proyectoExistente = await _context.Set<Proyecto>()
        //                                    .Include(i => i.Amenidades)
        //                                    .FirstOrDefaultAsync(i => i.Id == dto.Id);

        //    if (proyectoExistente == null)
        //    {
        //        throw new KeyNotFoundException($"Proyecto con ID {dto.Id} no encontrado.");
        //    }

        //    _mapper.Map(dto, proyectoExistente);

        //    var currentAmenidadIdsInDb = proyectoExistente.Amenidades?.Select(a => a.AmenidadProyectoId).ToList() ?? new List<int>();
        //    var newAmenidadDtos = dto.Amenidades ?? new List<AmenidadProyectoDTO>();
        //    var newAmenidadIdsFromDto = newAmenidadDtos.Select(a => a.AmenidadProyectoId).ToList();


        //    var amenidadesToRemove = proyectoExistente.Amenidades
        //                                .Where(a => !newAmenidadIdsFromDto.Contains(a.AmenidadProyectoId))
        //                                .ToList();

        //    foreach (var amenidad in amenidadesToRemove)
        //    {
        //        proyectoExistente.Amenidades.Remove(amenidad);

        //    }


        //    var amenidadesToAdd = newAmenidadDtos
        //                            .Where(newA => !currentAmenidadIdsInDb.Contains(newA.AmenidadProyectoId))
        //                            .Select(newA => new AmenidadProyecto
        //                            {
        //                                AmenidadProyectoId = newA.AmenidadProyectoId,
        //                                Nombre = newA.Nombre
        //                            })
        //                            .ToList();

        //    foreach (var amenidad in amenidadesToAdd)
        //    {
        //        proyectoExistente.Amenidades.Add(amenidad);
        //    }

        //    _context.Entry(proyectoExistente).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();
        //}

        public override async Task UpdateAsync(ProyectoDTO dto)
        {
            var proyectoExistente = await _context.Set<Proyecto>()
                                                .Include(i => i.Amenidades)
                                                .Include(i => i.ImagenesReferenciaProyecto) // This collection contains ImagenProyecto entities
                                                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (proyectoExistente == null)
            {
                throw new KeyNotFoundException($"Proyecto con ID {dto.Id} no encontrado.");
            }

            _mapper.Map(dto, proyectoExistente);

            // --- Lógica de sincronización para Amenidades ---
            if (dto.Amenidades != null)
            {
                var newAmenidadIdsFromDto = dto.Amenidades.Select(a => a.AmenidadProyectoId).ToList();

                var amenidadesToRemove = proyectoExistente.Amenidades
                                                            .Where(ai => !newAmenidadIdsFromDto.Contains(ai.AmenidadProyectoId))
                                                            .ToList();
                foreach (var amenidadProyecto in amenidadesToRemove)
                {
                    // This refers to the AmenidadProyecto entity
                    proyectoExistente.Amenidades.Remove(amenidadProyecto);
                }

                foreach (var amenidadDto in dto.Amenidades)
                {
                    if (!proyectoExistente.Amenidades.Any(ai => ai.AmenidadProyectoId == amenidadDto.AmenidadProyectoId))
                    {
                        var amenidadEntity = await _context.Set<Domain.Entities.Amenidad>().FirstOrDefaultAsync(a => a.Id == amenidadDto.AmenidadProyectoId);
                        if (amenidadEntity != null)
                        {
                            proyectoExistente.Amenidades.Add(new AmenidadProyecto
                            {
                                AmenidadProyectoId = amenidadEntity.Id,
                                ProyectoId = proyectoExistente.Id,
                                Nombre = amenidadEntity.Nombre
                            });
                        }
                    }
                }
            }
            else
            {
                proyectoExistente.Amenidades.Clear();
            }


            // --- Lógica de sincronización para ImagenesReferencia ---
            if (dto.ImagenesReferenciaProyecto != null)
            {
                var newImageUrlsFromDto = dto.ImagenesReferenciaProyecto.Select(img => img.Url).ToList();

                // `img` here will be of type `Domain.Entities.ImagenProyecto` because `proyectoExistente.ImagenesReferencia` is `ICollection<ImagenProyecto>`
                var imagesToRemove = proyectoExistente.ImagenesReferenciaProyecto
                                                        .Where(img => !newImageUrlsFromDto.Contains(img.Url))
                                                        .ToList();
                foreach (var img in imagesToRemove)
                {
                    // Line 198 (approx): Here, `img` is correctly `ImagenProyecto`.
                    // The error must have been from a previous mismatch, or a misinterpretation on my part of the exact line.
                    // This line _should_ now work because `img` is the correct entity type.
                    _context.Entry(img).State = EntityState.Deleted;
                }

                foreach (var imgDto in dto.ImagenesReferenciaProyecto)
                {
                    if (!proyectoExistente.ImagenesReferenciaProyecto.Any(img => img.Url == imgDto.Url))
                    {
                        // Here, create an instance of your **ENTITY** type: `ImagenProyecto`
                        proyectoExistente.ImagenesReferenciaProyecto.Add(new ImagenProyecto { Url = imgDto.Url, ProyectoId = proyectoExistente.Id });
                    }
                }
            }
            else
            {
                proyectoExistente.ImagenesReferenciaProyecto.Clear();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Set<Proyecto>().AnyAsync(e => e.Id == dto.Id))
                {
                    throw new KeyNotFoundException($"Proyecto con ID {dto.Id} no encontrado durante la actualización.");
                }
                else
                {
                    throw;
                }
            }
        }

        public override async Task DeleteAsync(object id)
        {
            var proyecto = await _context.Set<Proyecto>().FindAsync(id);
            if (proyecto == null)
            {
                throw new KeyNotFoundException($"Proyecto con ID {id} no encontrado para eliminar.");
            }
            _context.Set<Proyecto>().Remove(proyecto);
            await _context.SaveChangesAsync();
        }

        //Para arma el slug
        public async Task<Proyecto> GetBySlugAsync(string slugProyecto)
        {
            if (string.IsNullOrWhiteSpace(slugProyecto)) return null;

            // Por si viene URL-encoded o con espacios
            var s = Uri.UnescapeDataString(slugProyecto).Trim();

            // Normaliza si es mayus o minus
            // s = s.ToLowerInvariant();

            return await _context.Set<Proyecto>()
                .Include(i => i.ImagenesReferenciaProyecto)
                .Include(i => i.Amenidades)
                .FirstOrDefaultAsync(i => i.SlugProyecto == s);

        }

    }


}
