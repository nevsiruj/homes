using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Modules.ClienteModule.Services
{
    public interface IClienteService : IGenericService<ClienteDto>
    {
        Task<ClienteDto> GetClienteByDni(string dni);
        Task<List<ClienteDto>> GetAllByUser(string token);

        Task<ClienteDto> GetByIdAsync(object id);
        Task<CreateClienteDTO> CreateAsync(CreateClienteDTO cliente);
        Task<ClienteDto> UpdateAsync(ClienteDto clienteDto);


        Task<List<ClienteDto>> GetAllAsync();

        Task<ClienteDto> GetByTelefonoAsync(string telefono);
        Task UpdateStatusAsync(int id, EstadoCliente status);

    }


    public class ClienteService : GenericService<Cliente, ClienteDto>, IClienteService
    {
        private readonly ProjectDbContext _context;
        private readonly IAuthService _authService;

        public ClienteService(IGenericRepository<Cliente> repository, IMapper mapper, ProjectDbContext context, IAuthService authService)
            : base(repository, mapper)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ClienteDto> GetClienteByDni(string dni)
        {
            return _mapper.Map<ClienteDto>(_context.Clientes.FirstOrDefault(e => e.Dni == dni));
        }

        public override async Task<ClienteDto> GetByIdAsync(object id)
        {
            var cliente = await _context.Clientes
                .Include(e => e.Interacciones)
                .ThenInclude(a => a.Agente)
                // Se corrigió el Include para la colección de preferencias
                .Include(c => c.Preferencias)
                .ThenInclude(p => p.PreferenciaAmenidades)
                .FirstOrDefaultAsync(e => e.Id == (int)id);

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<CreateClienteDTO> CreateAsync(CreateClienteDTO cliente)
        {
            var clienteEntity = _mapper.Map<Cliente>(cliente);

            await _repository.CreateAsync(clienteEntity);
            await _repository.SaveChangesAsync();

            return cliente;
        }

        public async Task<List<ClienteDto>> GetAllByUser(string token)
        {
            var user = await _authService.GetLoggedUser(token);
            var principal = _authService.ValidateToken(token);
            if (principal == null)
            {
                throw new Exception("Not Authorized");
            }
            IQueryable<Cliente> query = _context.Clientes.Include(e => e.Interacciones);


            if (user.Rol == "Agente")
            {
                query = query.Where(v => v.AgenteId == user.Id);
            }
            var clientes = await query.ToListAsync();
            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto> UpdateAsync(ClienteDto clienteDto)
        {
            var clienteExistente = await _context.Clientes
                .Include(c => c.Preferencias)
                .ThenInclude(p => p.PreferenciaAmenidades)
                .Include(c => c.Interacciones)
                .FirstOrDefaultAsync(c => c.Id == clienteDto.Id);

            if (clienteExistente == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            _mapper.Map(clienteDto, clienteExistente);

            // lógica de interacciones …
            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteDto>(clienteExistente); // 👈 devolvemos el cliente actualizado
        }

      
        public async Task<List<ClienteDto>> GetAllAsync()
        {
            // Cargar solo los datos esenciales sin todas las relaciones
            // Las interacciones y amenidades se cargan solo cuando se necesitan (GetById)
            var clientes = await _context.Clientes
                .Include(n => n.Preferencias) // Solo preferencias básicas
                .OrderByDescending(c => c.FechaRegistro) // Ordenar por fecha más reciente primero
                .AsNoTracking() // Mejora el rendimiento al no rastrear cambios
                .ToListAsync();

            return _mapper.Map<List<ClienteDto>>(clientes);
        }
        // Fin

        public async Task<ClienteDto> GetByTelefonoAsync(string telefono)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Preferencias)
                .ThenInclude(p => p.PreferenciaAmenidades)
                .FirstOrDefaultAsync(c => c.Telefono == telefono);

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task UpdateStatusAsync(int id, EstadoCliente status)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                throw new KeyNotFoundException($"Cliente con ID {id} no encontrado.");
            }

            cliente.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}