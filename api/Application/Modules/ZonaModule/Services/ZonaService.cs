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
using System.Threading.Tasks;

namespace Application.Modules.ZonaModule.Services
{
    public interface IZonaService : IGenericService<ZonaDto>
    {
        Task<IEnumerable<ZonaDto>> GetAllActivasAsync();
        Task<IEnumerable<ZonaDto>> GetAllByCustomOrderAsync();
        Task<IEnumerable<ZonaDto>> GetAllActivasByCustomOrderAsync();
        Task ReorderZonasAsync(List<int> zonaIds);
    }

    public class ZonaService : GenericService<Zona, ZonaDto>, IZonaService
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;

        public ZonaService(
            IGenericRepository<Zona> repository,
            IMapper mapper,
            ProjectDbContext context)
            : base(repository, mapper, context)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task<ZonaDto> GetByIdAsync(object id)
        {
            var zona = await _context.Zonas.FindAsync((int)id);
            if (zona == null)
                throw new KeyNotFoundException($"Zona con ID {id} no encontrada.");
            return _mapper.Map<ZonaDto>(zona);
        }

        public override async Task<IEnumerable<ZonaDto>> GetAllAsync()
        {
            var zonas = await _context.Zonas
                .OrderBy(z => z.Orden)
                .ThenBy(z => z.Nombre)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ZonaDto>>(zonas);
        }

        public async Task<IEnumerable<ZonaDto>> GetAllByCustomOrderAsync()
        {
            var zonas = await _context.Zonas
                .OrderBy(z => z.Orden)
                .ThenBy(z => z.Nombre)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ZonaDto>>(zonas);
        }

        public async Task<IEnumerable<ZonaDto>> GetAllActivasAsync()
        {
            var zonas = await _context.Zonas
                .Where(z => z.Activo)
                .OrderBy(z => z.Orden)
                .ThenBy(z => z.Nombre)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ZonaDto>>(zonas);
        }

        public async Task<IEnumerable<ZonaDto>> GetAllActivasByCustomOrderAsync()
        {
            var zonas = await _context.Zonas
                .Where(z => z.Activo)
                .OrderBy(z => z.Orden)
                .ThenBy(z => z.Nombre)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ZonaDto>>(zonas);
        }

        public async Task ReorderZonasAsync(List<int> zonaIds)
        {
            if (zonaIds == null || zonaIds.Count == 0)
                throw new ArgumentException("La lista de IDs de zonas no puede estar vacía.");

            var zonas = await _context.Zonas
                .Where(z => zonaIds.Contains(z.Id))
                .ToListAsync();

            if (zonas.Count != zonaIds.Count)
                throw new KeyNotFoundException("Una o más zonas no existen.");

            // Asignar el nuevo orden basado en la posición en la lista
            for (int i = 0; i < zonaIds.Count; i++)
            {
                var zona = zonas.FirstOrDefault(z => z.Id == zonaIds[i]);
                if (zona != null)
                {
                    zona.Orden = i + 1;
                }
            }

            _context.Zonas.UpdateRange(zonas);
            await _context.SaveChangesAsync();
        }

        public override async Task<ZonaDto> CreateAsync(ZonaDto zonaDto)
        {
            if (string.IsNullOrWhiteSpace(zonaDto.Nombre))
                throw new ArgumentException("El nombre de la zona es obligatorio.");

            var existente = await _context.Zonas
                .FirstOrDefaultAsync(z => z.Nombre.ToLower() == zonaDto.Nombre.ToLower());

            if (existente != null)
                throw new InvalidOperationException($"Ya existe una zona llamada '{zonaDto.Nombre}'.");

            // Si no se proporciona orden, asignar el siguiente número de orden
            int proximoOrden = zonaDto.Orden > 0 ? zonaDto.Orden : (await _context.Zonas.MaxAsync(z => (int?)z.Orden) ?? 0) + 1;

            var zona = new Zona
            {
                Nombre = zonaDto.Nombre.Trim(),
                Activo = zonaDto.Activo,
                Orden = proximoOrden
            };

            await _context.Zonas.AddAsync(zona);
            await _context.SaveChangesAsync();

            return _mapper.Map<ZonaDto>(zona);
        }

        public override async Task UpdateAsync(ZonaDto zonaDto)
        {
            var zona = await _context.Zonas.FindAsync(zonaDto.Id);
            if (zona == null)
                throw new KeyNotFoundException($"Zona con ID {zonaDto.Id} no encontrada.");

            if (string.IsNullOrWhiteSpace(zonaDto.Nombre))
                throw new ArgumentException("El nombre de la zona es obligatorio.");

            var existente = await _context.Zonas
                .FirstOrDefaultAsync(z => z.Id != zonaDto.Id && z.Nombre.ToLower() == zonaDto.Nombre.ToLower());

            if (existente != null)
                throw new InvalidOperationException($"Ya existe una zona llamada '{zonaDto.Nombre}'.");

            zona.Nombre = zonaDto.Nombre.Trim();
            zona.Activo = zonaDto.Activo;
            zona.Orden = zonaDto.Orden;

            _context.Zonas.Update(zona);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(object id)
        {
            var zona = await _context.Zonas.FindAsync((int)id);
            if (zona == null)
                throw new KeyNotFoundException($"Zona con ID {id} no encontrada.");

            _context.Zonas.Remove(zona);
            await _context.SaveChangesAsync();
        }
    }
}
