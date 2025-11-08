using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Modules.MatchModule.Services; 

namespace Application.Modules.PreferenciaModule.Service
{
    public interface IPreferenciaService
    {
        Task<List<PreferenciaDTO>> GetByClienteIdAsync(int clienteId);
        Task<PreferenciaDTO> AddOrUpdateAsync(int clienteId, CreatePreferenciaDTO nuevaPreferenciaDto);
        Task<Preferencias> AddAsync(CreatePreferenciaDTO nuevaPreferenciaDto);
        Task UpdateAsync(int preferenciaId, PreferenciaDTO preferenciaDto);
        Task PatchAsync(int preferenciaId, PreferenciaDTO preferenciaDto);
        Task DeleteAsync(int preferenciaId);
    }

    public class PreferenciaService : IPreferenciaService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMatchService _matchService; // <- añadido

        public PreferenciaService(ProjectDbContext context, IMapper mapper, IMatchService matchService) // <- constructor actualizado
        {
            _context = context;
            _mapper = mapper;
            _matchService = matchService;
        }

        public async Task<List<PreferenciaDTO>> GetByClienteIdAsync(int clienteId)
        {
            var preferencias = await _context.Preferencias
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.PreferenciaAmenidades)
                .ToListAsync();

            return _mapper.Map<List<PreferenciaDTO>>(preferencias);
        }

        public async Task<PreferenciaDTO> AddOrUpdateAsync(int clienteId, CreatePreferenciaDTO nuevaPreferenciaDto)
        {
            var preferenciaExistente = await _context.Preferencias
                .Include(p => p.PreferenciaAmenidades)
                .FirstOrDefaultAsync(p => p.ClienteId == clienteId);

            var amenidadesUnicas = nuevaPreferenciaDto.PreferenciaAmenidades?
                .GroupBy(a => a.AmenidadId > 0 ? a.AmenidadId : (object)a.Nombre.ToLowerInvariant())
                .Select(g => g.First())
                .ToList() ?? new List<CreatePreferenciaAmenidadDTO>();

            if (preferenciaExistente != null)
            {
                _mapper.Map(nuevaPreferenciaDto, preferenciaExistente);

                _context.PreferenciaAmenidades.RemoveRange(preferenciaExistente.PreferenciaAmenidades);
                await _context.SaveChangesAsync();

                preferenciaExistente.PreferenciaAmenidades = amenidadesUnicas.Select(amenidadDto => new PreferenciaAmenidad
                {
                    AmenidadId = amenidadDto.AmenidadId,
                    Nombre = amenidadDto.Nombre,
                    PreferenciasId = preferenciaExistente.Id
                }).ToList();

                await _context.SaveChangesAsync();

                // Sincronizar matches para el cliente
                if (preferenciaExistente.ClienteId.HasValue)
                {
                    await _matchService.RefreshMatchesForClienteAsync(preferenciaExistente.ClienteId.Value);
                }

                return _mapper.Map<PreferenciaDTO>(preferenciaExistente);
            }
            else
            {
                var nuevaPreferencia = _mapper.Map<Preferencias>(nuevaPreferenciaDto);
                nuevaPreferencia.ClienteId = clienteId;

                nuevaPreferencia.PreferenciaAmenidades = amenidadesUnicas.Select(amenidadDto => new PreferenciaAmenidad
                {
                    AmenidadId = amenidadDto.AmenidadId,
                    Nombre = amenidadDto.Nombre
                }).ToList();

                _context.Preferencias.Add(nuevaPreferencia);
                await _context.SaveChangesAsync();

                // Sincronizar matches para el cliente
                await _matchService.RefreshMatchesForClienteAsync(clienteId);

                return _mapper.Map<PreferenciaDTO>(nuevaPreferencia);
            }
        }

        public async Task<Preferencias> AddAsync(CreatePreferenciaDTO nuevaPreferenciaDto)
        {
            var nuevaPreferencia = _mapper.Map<Preferencias>(nuevaPreferenciaDto);

            var amenidadesUnicas = nuevaPreferenciaDto.PreferenciaAmenidades?
                .GroupBy(a => a.AmenidadId > 0 ? a.AmenidadId : (object)a.Nombre.ToLowerInvariant())
                .Select(g => g.First())
                .ToList() ?? new List<CreatePreferenciaAmenidadDTO>();

            nuevaPreferencia.PreferenciaAmenidades = amenidadesUnicas.Select(amenidadDto => new PreferenciaAmenidad
            {
                AmenidadId = amenidadDto.AmenidadId,
                Nombre = amenidadDto.Nombre
            }).ToList();

            _context.Preferencias.Add(nuevaPreferencia);
            await _context.SaveChangesAsync();

            if (nuevaPreferencia.ClienteId.HasValue)
            {
                await _matchService.RefreshMatchesForClienteAsync(nuevaPreferencia.ClienteId.Value);
            }

            return nuevaPreferencia;
        }

        public async Task UpdateAsync(int preferenciaId, PreferenciaDTO preferenciaDto)
        {
            var preferenciaExistente = await _context.Preferencias
                .Include(p => p.PreferenciaAmenidades)
                .FirstOrDefaultAsync(p => p.Id == preferenciaId);

            if (preferenciaExistente == null)
            {
                throw new KeyNotFoundException("Preferencia no encontrada.");
            }

            preferenciaDto.Id = preferenciaId;
            preferenciaDto.ClienteId = (int)preferenciaExistente.ClienteId;

            preferenciaExistente.Tipo = preferenciaDto.Tipo ?? string.Empty;
            preferenciaExistente.Operacion = preferenciaDto.Operacion; 
            preferenciaExistente.Ubicacion = preferenciaDto.Ubicacion;
            preferenciaExistente.PrecioMin = preferenciaDto.PrecioMin ?? 0;
            preferenciaExistente.PrecioMax = preferenciaDto.PrecioMax ?? 0;
            preferenciaExistente.Habitaciones = preferenciaDto.Habitaciones ?? 0;
            preferenciaExistente.Banos = preferenciaDto.Banos ?? 0;
            preferenciaExistente.MetrosCuadrados = preferenciaDto.MetrosCuadrados ?? 0;
            preferenciaExistente.Comentarios = preferenciaDto.Comentarios;

            _context.PreferenciaAmenidades.RemoveRange(preferenciaExistente.PreferenciaAmenidades);

            if (preferenciaDto.PreferenciaAmenidades != null && preferenciaDto.PreferenciaAmenidades.Any())
            {
                var amenidadesUnicas = preferenciaDto.PreferenciaAmenidades
                    .GroupBy(a => a.AmenidadId > 0 ? a.AmenidadId : (object)a.Nombre.ToLowerInvariant())
                    .Select(g => g.First())
                    .ToList();

                preferenciaExistente.PreferenciaAmenidades = amenidadesUnicas.Select(amenidadDto => new PreferenciaAmenidad
                {
                    AmenidadId = amenidadDto.AmenidadId,
                    Nombre = amenidadDto.Nombre,
                    PreferenciasId = preferenciaExistente.Id
                }).ToList();
            }
            else
            {
                preferenciaExistente.PreferenciaAmenidades.Clear();
            }

            await _context.SaveChangesAsync();

            // Sincronizar matches para el cliente
            if (preferenciaExistente.ClienteId.HasValue)
            {
                await _matchService.RefreshMatchesForClienteAsync(preferenciaExistente.ClienteId.Value);
            }
        }

        public async Task PatchAsync(int preferenciaId, PreferenciaDTO preferenciaDto)
        {
            var preferenciaExistente = await _context.Preferencias
                .Include(p => p.PreferenciaAmenidades)
                .FirstOrDefaultAsync(p => p.Id == preferenciaId);

            if (preferenciaExistente == null)
                throw new KeyNotFoundException("Preferencia no encontrada.");

            preferenciaExistente.Tipo = preferenciaDto.Tipo ?? preferenciaExistente.Tipo;
            preferenciaExistente.Operacion = preferenciaDto.Operacion ?? preferenciaExistente.Operacion;
            preferenciaExistente.Ubicacion = preferenciaDto.Ubicacion ?? preferenciaExistente.Ubicacion;
            preferenciaExistente.PrecioMin = preferenciaDto.PrecioMin ?? preferenciaExistente.PrecioMin;
            preferenciaExistente.PrecioMax = preferenciaDto.PrecioMax ?? preferenciaExistente.PrecioMax;
            preferenciaExistente.Habitaciones = preferenciaDto.Habitaciones ?? preferenciaExistente.Habitaciones;
            preferenciaExistente.Banos = preferenciaDto.Banos ?? preferenciaExistente.Banos;
            preferenciaExistente.MetrosCuadrados = preferenciaDto.MetrosCuadrados ?? preferenciaExistente.MetrosCuadrados;
            preferenciaExistente.Comentarios = preferenciaDto.Comentarios ?? preferenciaExistente.Comentarios;

            if (preferenciaDto.PreferenciaAmenidades != null)
            {
                _context.PreferenciaAmenidades.RemoveRange(preferenciaExistente.PreferenciaAmenidades);

                var amenidadesUnicas = preferenciaDto.PreferenciaAmenidades
                    .GroupBy(a => a.AmenidadId > 0 ? a.AmenidadId : (object)a.Nombre.ToLowerInvariant())
                    .Select(g => g.First())
                    .ToList();

                preferenciaExistente.PreferenciaAmenidades = amenidadesUnicas.Select(amenidadDto => new PreferenciaAmenidad
                {
                    AmenidadId = amenidadDto.AmenidadId,
                    Nombre = amenidadDto.Nombre,
                    PreferenciasId = preferenciaExistente.Id
                }).ToList();
            }

            await _context.SaveChangesAsync();

            // Sincronizar matches para el cliente
            if (preferenciaExistente.ClienteId.HasValue)
            {
                await _matchService.RefreshMatchesForClienteAsync(preferenciaExistente.ClienteId.Value);
            }
        }

        public async Task DeleteAsync(int preferenciaId)
        {
            var preferencia = await _context.Preferencias.FindAsync(preferenciaId);
            if (preferencia != null)
            {
                var clienteId = preferencia.ClienteId;
                _context.Preferencias.Remove(preferencia);
                await _context.SaveChangesAsync();

                // Sincronizar matches para el cliente (si existía)
                if (clienteId.HasValue)
                {
                    await _matchService.RefreshMatchesForClienteAsync(clienteId.Value);
                }
            }
        }
    }
}