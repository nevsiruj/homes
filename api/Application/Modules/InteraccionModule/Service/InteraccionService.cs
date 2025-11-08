using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
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

namespace Application.Modules.InteraccionModule.Service
{
    public interface IInteraccionService
    {
        Task<List<Interaccion>> ObtenerTodasAsync();
        Task<Interaccion> ObtenerPorIdAsync(int id);
        Task CrearAsync(Interaccion interaccion);
        Task ActualizarAsync(Interaccion interaccion);
        Task EliminarAsync(int id);
        Task ActualizarStatusAsync(int id, string nuevoStatus);
    }
    public class InteraccionService : IInteraccionService
    {
        private readonly ProjectDbContext _context;

        public InteraccionService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Interaccion>> ObtenerTodasAsync()
        {
            return await _context.Interacciones.ToListAsync();
        }

        public async Task<Interaccion> ObtenerPorIdAsync(int id)
        {
            return await _context.Interacciones.FindAsync(id);
        }

        public async Task CrearAsync(Interaccion interaccion)
        {
            _context.Interacciones.Add(interaccion);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Interaccion interaccion)
        {
            _context.Interacciones.Update(interaccion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var interaccion = await _context.Interacciones.FindAsync(id);
            if (interaccion != null)
            {
                _context.Interacciones.Remove(interaccion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActualizarStatusAsync(int id, string nuevoStatus)
        {
            var interaccion = await _context.Interacciones.FindAsync(id);
            if (interaccion != null)
            {
                interaccion.StatusInteraccion = nuevoStatus;
                _context.Interacciones.Update(interaccion);
                await _context.SaveChangesAsync();
            }
        }
    }
}