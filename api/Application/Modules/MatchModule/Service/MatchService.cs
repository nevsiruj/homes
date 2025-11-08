using AlmacenSystem.Infrastructure.Repositories.Common;
using Application.Modules.InmuebleModule.Services;
using AutoMapper;
using Domain.DTOs;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Application.Modules.MatchModule.Services
{

    public interface IMatchService : IGenericService<MatchDto>
    {

        Task<MatchDto> GetByIdAsync(object id);
        Task<MatchDto> CreateAsync(MatchDto matchDto);
        Task UpdateAsync(MatchDto matchDto);
        Task DeleteAsync(object id);

        //Task<IEnumerable<MatchDto>> GetPendientesByClienteIdAsync(int clienteId);
        Task<IEnumerable<MatchDto>> GetEnviadosByClienteIdAsync(int clienteId);
        Task MarcarComoEnviadoAsync(int matchId);
        Task<List<MatchSugeridoDto>> GenerarMatchesSugeridos(int clienteId);

        // Nuevo: Fuerza la sincronización (refresca) de la tabla Matches para un cliente
        Task RefreshMatchesForClienteAsync(int clienteId);
    }

    public class MatchService : GenericService<Domain.Entities.Match, MatchDto>, IMatchService
    {
        private readonly ProjectDbContext _context;
        private const decimal MATCH_THRESHOLD = 60m; // umbral utilizado para crear/retener matches

        public MatchService(IGenericRepository<Domain.Entities.Match> repository, IMapper mapper, ProjectDbContext context)
            : base(repository, mapper, context)
        {
            _context = context;
        }

        // --- cruds ---

        public override async Task<MatchDto> GetByIdAsync(object id)
        {
            var match = await _context.Matches.FindAsync((int)id);
            return _mapper.Map<MatchDto>(match);
        }


        public override async Task DeleteAsync(object id)
        {
            var match = await _context.Matches.FindAsync((int)id);
            if (match == null)
                throw new KeyNotFoundException($"Match con ID {id} no encontrado para eliminar.");

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }

        public async Task<MatchDto> CreateAsync(MatchDto matchDto)
        {
            // 1. **VALIDACIÓN CRUCIAL:** Verificar que el ClienteId e InmuebleId existen.

            // NOTA: Asumo que MatchDto contiene ClienteId e InmuebleId
            if (matchDto.ClienteId == 0 || matchDto.InmuebleId == 0)
            {
                throw new ArgumentException("ClienteId e InmuebleId son obligatorios.");
            }

            // Verificar existencia del Cliente
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == matchDto.ClienteId);
            if (!clienteExiste)
            {
                // Lanza una excepción clara para el desarrollador/front-end
                throw new KeyNotFoundException($"El Cliente con ID {matchDto.ClienteId} no existe. No se puede crear el Match.");
            }

            // Verificar existencia del Inmueble
            var inmuebleExiste = await _context.Inmuebles.AnyAsync(i => i.Id == matchDto.InmuebleId);
            if (!inmuebleExiste)
            {
                // Lanza una excepción clara para el desarrollador/front-end
                throw new KeyNotFoundException($"El Inmueble con ID {matchDto.InmuebleId} no existe. No se puede crear el Match.");
            }

            // 2. Mapeo y asignación de valores
            var matchEntity = _mapper.Map<Domain.Entities.Match>(matchDto);

            // Valores por defecto
            matchEntity.EsEnviado = false;
            matchEntity.CreatedAt = DateTime.UtcNow;

            // 3. Persistencia (Guardado)
            await _context.Matches.AddAsync(matchEntity);

            // El SaveChangesAsync está ahora protegido por las validaciones.
            // Si falla aquí, es por un problema de concurrencia o de DB muy atípico.
            await _context.SaveChangesAsync();

            return _mapper.Map<MatchDto>(matchEntity);
        }

        public async Task UpdateAsync(MatchDto matchDto)
        {
            var matchExistente = await _context.Matches.FindAsync(matchDto.Id);
            if (matchExistente == null)
            {
                throw new KeyNotFoundException("Match no encontrado.");
            }
            _mapper.Map(matchDto, matchExistente);
            _context.Matches.Update(matchExistente);
            await _context.SaveChangesAsync();
        }


        // --- logica de match ---


        // --- estados ---

        public async Task<IEnumerable<MatchDto>> GetEnviadosByClienteIdAsync(int clienteId)
        {
            var matchesEnviados = await _context.Matches
                .Where(m => m.ClienteId == clienteId && m.EsEnviado == true)
                .OrderByDescending(m => m.FechaEnvio)
                .Include(m => m.Inmueble)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MatchDto>>(matchesEnviados);
        }

        //Match

        public string NormalizarOperacion(string operacion)
        {
            if (string.IsNullOrEmpty(operacion)) return string.Empty;

            string normalizada = operacion.Trim().ToLowerInvariant();

            if (normalizada == "alquiler" || normalizada == "renta")
            {
                return "Renta"; // Usamos 'Renta' como el estándar
            }
            // Si manejas otros idiomas o términos, añádelos aquí
            return normalizada;
        }
        public async Task MarcarComoEnviadoAsync(int matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new KeyNotFoundException($"Match con ID {matchId} no encontrado.");
            }
            match.EsEnviado = true;
            match.FechaEnvio = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // PUBLIC: refresca la tabla de Matches para un cliente según sus preferencias actuales.
        public async Task RefreshMatchesForClienteAsync(int clienteId)
        {
            // Cargar cliente y preferencias (incluyendo amenidades)
            var cliente = await _context.Clientes
                .Include(c => c.Preferencias)
                    .ThenInclude(p => p.PreferenciaAmenidades)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            // Obtener matches pendientes existentes (no enviados)
            var matchesPendientes = await _context.Matches
                .Where(m => m.ClienteId == clienteId && !m.EsEnviado)
                .ToListAsync();

            // Si no existen preferencias -> eliminar todos los matches pendientes del cliente
            if (cliente?.Preferencias == null || !cliente.Preferencias.Any())
            {
                if (matchesPendientes.Any())
                {
                    _context.Matches.RemoveRange(matchesPendientes);
                    await _context.SaveChangesAsync();
                }
                return;
            }

            // Obtener propiedades activas (solo las que pueden ser candidateadas)
            var inmuebles = await _context.Inmuebles
                .Where(i => i.PrecioActivo == true)
                .Include(i => i.Amenidades)
                .AsNoTracking()
                .ToListAsync();

            // Mapear inmemoria para acceso rápido
            var inmueblesDict = inmuebles.ToDictionary(i => i.Id);

            // 1) Eliminar matches pendientes que ya no cumplen (puntuacion < MATCH_THRESHOLD o propiedad inexistente)
            var matchesToRemove = new List<Domain.Entities.Match>();
            foreach (var match in matchesPendientes)
            {
                if (!inmueblesDict.TryGetValue(match.InmuebleId, out var prop) )
                {
                    // propiedad eliminada -> eliminar match pendiente
                    matchesToRemove.Add(match);
                    continue;
                }

                var (puntuacion, _) = CalcularPuntuacionMatchConAmenidades(cliente.Preferencias, prop);
                if (puntuacion < MATCH_THRESHOLD)
                {
                    matchesToRemove.Add(match);
                }
            }

            if (matchesToRemove.Any())
            {
                _context.Matches.RemoveRange(matchesToRemove);
            }

            // 2) Crear matches pendientes nuevos para propiedades que cumplan y que no existan (ni pendientes ni enviados)
            // Obtener IDs ya presentes (pendientes + enviados) para no duplicar
            var inmueblesExistentesIds = await _context.Matches
                .Where(m => m.ClienteId == clienteId)
                .Select(m => m.InmuebleId)
                .ToListAsync();

            var matchesToAdd = new List<Domain.Entities.Match>();

            foreach (var prop in inmuebles)
            {
                if (inmueblesExistentesIds.Contains(prop.Id)) continue; // ya existe (pendiente o enviado)

                var (puntuacion, _) = CalcularPuntuacionMatchConAmenidades(cliente.Preferencias, prop);
                if (puntuacion >= MATCH_THRESHOLD)
                {
                    matchesToAdd.Add(new Domain.Entities.Match
                    {
                        ClienteId = clienteId,
                        InmuebleId = prop.Id,
                        CodigoPropiedad = prop.CodigoPropiedad,
                        EsEnviado = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            if (matchesToAdd.Any())
            {
                await _context.Matches.AddRangeAsync(matchesToAdd);
            }

            // Persistir todos los cambios en una única transacción
            if (matchesToRemove.Any() || matchesToAdd.Any())
            {
                await _context.SaveChangesAsync();
            }
        }

        // GenerarMatchesSugeridos ahora solicita refrescar primero la tabla para garantizar coherencia.
        public async Task<List<MatchSugeridoDto>> GenerarMatchesSugeridos(int clienteId)
        {
            // Sincronizar tabla Matches con preferencias actuales del cliente
            await RefreshMatchesForClienteAsync(clienteId);

            // 1. Obtener cliente con preferencias
            var cliente = await _context.Clientes
                .Include(c => c.Preferencias)
                    .ThenInclude(p => p.PreferenciaAmenidades)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente?.Preferencias == null || !cliente.Preferencias.Any())
            {
                return new List<MatchSugeridoDto>();
            }

            // 2. Obtener IDs de inmuebles ya enviados
            var inmueblesEnviadosIds = await _context.Matches
                .Where(m => m.ClienteId == clienteId && m.EsEnviado)
                .Select(m => m.InmuebleId)
                .ToListAsync();

            // 3. Obtener propiedades activas
            var propiedades = await _context.Inmuebles
                .Where(i => i.PrecioActivo == true && !inmueblesEnviadosIds.Contains(i.Id))
                .Include(i => i.Amenidades)
                .AsNoTracking()
                .ToListAsync();

            // 4. Obtener matches pendientes existentes
            var matchesPendientes = await _context.Matches
                .Where(m => m.ClienteId == clienteId && !m.EsEnviado)
                .ToListAsync();

            // 5. IDENTIFICAR NUEVOS MATCHES (que no existen como pendientes)
            var propiedadesParaNuevosMatches = propiedades
                .Where(p => !matchesPendientes.Any(mp => mp.InmuebleId == p.Id))
                .ToList();

            // 6. CREAR NUEVOS MATCHES EN BD (seguir la lógica de negocio existente)
            var nuevosMatches = new List<Domain.Entities.Match>();

            foreach (var propiedad in propiedadesParaNuevosMatches)
            {
                var (puntuacion, amenidadesCoincidentes) = CalcularPuntuacionMatchConAmenidades(cliente.Preferencias, propiedad);

                if (puntuacion >= MATCH_THRESHOLD)
                {
                    nuevosMatches.Add(new Domain.Entities.Match
                    {
                        ClienteId = clienteId,
                        InmuebleId = propiedad.Id,
                        CodigoPropiedad = propiedad.CodigoPropiedad,
                        EsEnviado = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            // 7. PERSISTIR NUEVOS MATCHES
            if (nuevosMatches.Any())
            {
                await _context.Matches.AddRangeAsync(nuevosMatches);
                await _context.SaveChangesAsync();

                // Actualizar la lista de matches pendientes
                matchesPendientes = await _context.Matches
                    .Where(m => m.ClienteId == clienteId && !m.EsEnviado)
                    .ToListAsync();
            }

            // 8. GENERAR DTOs PARA TODOS LOS MATCHES PENDIENTES (EXISTENTES)
            var matchesSugeridos = new List<MatchSugeridoDto>();

            foreach (var match in matchesPendientes)
            {
                var propiedad = propiedades.FirstOrDefault(p => p.Id == match.InmuebleId);
                if (propiedad != null)
                {
                    var (puntuacion, amenidadesCoincidentes) = CalcularPuntuacionMatchConAmenidades(cliente.Preferencias, propiedad);

                    matchesSugeridos.Add(new MatchSugeridoDto
                    {
                        PropiedadId = propiedad.Id,
                        CodigoPropiedad = propiedad.CodigoPropiedad,
                        Titulo = propiedad.Titulo,
                        Slug = propiedad.SlugInmueble,
                        Ubicacion = propiedad.Ubicaciones,
                        Precio = propiedad.Precio,

                        // ✅ CORRECCIÓN 1: Asignación explícita de Habitaciones y Baños del Inmueble
                        // Esto asegura que se tomen los valores de la entidad, que son 'int' (no null)
                        Habitaciones = propiedad.Habitaciones,
                        Banos = propiedad.Banos,

                        PuntuacionMatch = puntuacion,
                        Coincidencias = ObtenerCoincidenciasDetalladas(cliente.Preferencias, propiedad),
                        Amenidades = propiedad.Amenidades?.Select(a => a.Nombre).ToList() ?? new List<string>(),
                        AmenidadesCoincidentes = amenidadesCoincidentes,
                        EsPendiente = true, // Ya existe en BD
                        YaEnviado = false,
                        MatchId = match.Id // ID del match existente
                    });
                }
            }

            // 9. AGREGAR PROPIEDADES QUE CUMPLEN CRITERIOS PERO NO TIENEN MATCH CREADO
            var propiedadesSugeridasSinMatch = new List<MatchSugeridoDto>();

            foreach (var propiedad in propiedadesParaNuevosMatches)
            {
                var (puntuacion, amenidadesCoincidentes) = CalcularPuntuacionMatchConAmenidades(cliente.Preferencias, propiedad);

                if (puntuacion >= MATCH_THRESHOLD)
                {
                    propiedadesSugeridasSinMatch.Add(new MatchSugeridoDto
                    {
                        PropiedadId = propiedad.Id,
                        CodigoPropiedad = propiedad.CodigoPropiedad,
                        Titulo = propiedad.Titulo,
                        Slug = propiedad.SlugInmueble,
                        Ubicacion = propiedad.Ubicaciones,
                        Precio = propiedad.Precio,

                        // ✅ CORRECCIÓN 2: Asignación explícita de Habitaciones y Baños del Inmueble
                        Habitaciones = propiedad.Habitaciones,
                        Banos = propiedad.Banos,

                        PuntuacionMatch = puntuacion,
                        Coincidencias = ObtenerCoincidenciasDetalladas(cliente.Preferencias, propiedad),
                        Amenidades = propiedad.Amenidades?.Select(a => a.Nombre).ToList() ?? new List<string>(),
                        AmenidadesCoincidentes = amenidadesCoincidentes,
                        EsPendiente = false, // No existe match en BD aún
                        YaEnviado = false,
                        MatchId = null // No tiene ID de match
                    });
                }
            }

            // 10. COMBINAR AMBAS LISTAS
            matchesSugeridos.AddRange(propiedadesSugeridasSinMatch);

            return matchesSugeridos
                .OrderByDescending(m => m.PuntuacionMatch)
                .Take(50)
                .ToList();
        }
        private (decimal Puntuacion, List<string> AmenidadesCoincidentes) CalcularPuntuacionMatchConAmenidades(
        ICollection<Domain.Entities.Preferencias> preferencias,
        Domain.Entities.Inmueble propiedad)
        {
            var mejorPuntuacion = 0m;
            var mejorAmenidadesCoincidentes = new List<string>();

            var ubicacionPropiedadNormalizada = propiedad.Ubicaciones?.Trim().ToUpper() ?? string.Empty;

            foreach (var pref in preferencias)
            {
                var puntuacion = 0m;
                var amenidadesCoincidentes = new List<string>();

                // =========================================================================
                // ✅ FILTROS OBLIGATORIOS ESTRICTOS (Cláusulas de Guardia)
                // =========================================================================

                // 1. FILTRO ESTRICTO DE TIPO
                // Nota: Asumiendo que 'Tipos' en Inmueble es un campo de texto que contiene el tipo,
                // o que la comparación es case-insensitive y sin normalizar.
                if (!string.IsNullOrEmpty(pref.Tipo) &&
                    (propiedad.Tipos?.Trim().ToUpper() != pref.Tipo.Trim().ToUpper()))
                {
                    continue;
                }

                // 2. FILTRO ESTRICTO DE OPERACIÓN (CORREGIDO para usar NormalizarOperacion)
                if (!string.IsNullOrEmpty(pref.Operacion))
                {
                    var operacionPreferenciaNormalizada = NormalizarOperacion(pref.Operacion);
                    var operacionPropiedadNormalizada = NormalizarOperacion(propiedad.Operaciones);

                    if (operacionPropiedadNormalizada != operacionPreferenciaNormalizada)
                    {
                        continue;
                    }
                }

                // 3. FILTRO ESTRICTO DE HABITACIONES (Debe ser IGUAL O SUPERIOR)
                if (pref.Habitaciones.HasValue && pref.Habitaciones.Value > 0)
                {
                    if (propiedad.Habitaciones < pref.Habitaciones.Value)
                    {
                        continue;
                    }
                }

                // 4. FILTRO ESTRICTO DE BAÑOS (Debe ser IGUAL O SUPERIOR)
                if (pref.Banos.HasValue && pref.Banos.Value > 0)
                {
                    if (propiedad.Banos < pref.Banos.Value)
                    {
                        continue;
                    }
                }

                // 5. FILTRO ESTRICTO DE UBICACIÓN/ZONA
                if (!string.IsNullOrEmpty(pref.Ubicacion))
                {
                    var ubicacionesPreferidas = pref.Ubicacion.Split(',')
                        .Select(u => u.Trim().ToUpper())
                        .Where(u => !string.IsNullOrEmpty(u))
                        .ToList();

                    if (!ubicacionesPreferidas.Any(up => ubicacionPropiedadNormalizada.Contains(up)))
                    {
                        continue;
                    }
                }

                // 6. ✅ FILTRO ESTRICTO DE PRECIO
                decimal? prefMin = (pref.PrecioMin.HasValue && pref.PrecioMin.Value > 0) ? pref.PrecioMin : null;
                decimal? prefMax = (pref.PrecioMax.HasValue && pref.PrecioMax.Value > 0) ? pref.PrecioMax : null;

                if (prefMin.HasValue && prefMax.HasValue)
                {
                    if (propiedad.Precio < prefMin.Value || propiedad.Precio > prefMax.Value)
                    {
                        continue; // Descartar si el precio está fuera del rango min/max
                    }
                }
                else if (prefMin.HasValue)
                {
                    if (propiedad.Precio < prefMin.Value)
                    {
                        continue;
                    }
                }
                else if (prefMax.HasValue)
                {
                    if (propiedad.Precio > prefMax.Value)
                    {
                        continue;
                    }
                }
                // =========================================================================

                // --- CÁLCULO DE PUNTUACIÓN (Solo si la propiedad ha pasado todos los filtros) ---

                // 1. Tipo (20%) - Gana puntos por pasar el filtro
                puntuacion += 20;

                // 2. Operación (20%) - Gana puntos por pasar el filtro
                puntuacion += 20;

                // 3. Precio (15%) - Gana puntos por pasar el filtro estricto (si se aplicó)
                puntuacion += 15;

                // 4. Habitaciones (5%) - Gana puntos por pasar el filtro
                puntuacion += 5;
                // Penalización por exceder el número deseado por más de 2 unidades
                if (pref.Habitaciones.HasValue && pref.Habitaciones.Value > 0 && propiedad.Habitaciones > pref.Habitaciones.Value + 2)
                {
                    puntuacion -= 2.5m;
                }

                // 5. Baños (5%) - Gana puntos por pasar el filtro
                puntuacion += 5;
                // Penalización por exceder el número deseado por más de 2 unidades
                if (pref.Banos.HasValue && pref.Banos.Value > 0 && propiedad.Banos > pref.Banos.Value + 2)
                {
                    puntuacion -= 2.5m;
                }

                // 6. Ubicación (15%) - Gana puntos por pasar el filtro
                puntuacion += 15;

                // 7. Amenidades (20%)
                if (pref.PreferenciaAmenidades?.Any() == true && propiedad.Amenidades?.Any() == true)
                {
                    var amenidadesPropiedad = propiedad.Amenidades
                        .Select(a => a.Nombre.Trim().ToUpper())
                        .ToHashSet();

                    var amenidadesPreferidas = pref.PreferenciaAmenidades
                        .Select(pa => pa.Nombre.Trim().ToUpper())
                        .ToList();

                    // Calcular coincidencias exactas y flexibles
                    foreach (var amenidadPreferida in amenidadesPreferidas)
                    {
                        if (amenidadesPropiedad.Contains(amenidadPreferida))
                        {
                            amenidadesCoincidentes.Add(amenidadPreferida);
                        }
                        else
                        {
                            // Búsqueda flexible (por si hay nombres similares)
                            var coincidenciaFlexible = amenidadesPropiedad.FirstOrDefault(ap =>
                                ap.Contains(amenidadPreferida) || amenidadPreferida.Contains(ap));

                            if (coincidenciaFlexible != null && !amenidadesCoincidentes.Contains(coincidenciaFlexible))
                            {
                                amenidadesCoincidentes.Add(coincidenciaFlexible);
                            }
                        }
                    }

                    var porcentaje = (amenidadesCoincidentes.Count / (double)pref.PreferenciaAmenidades.Count) * 20;
                    puntuacion += (decimal)porcentaje;
                }

                // Actualizar mejor puntuación
                if (puntuacion > mejorPuntuacion)
                {
                    mejorPuntuacion = puntuacion;
                    mejorAmenidadesCoincidentes = amenidadesCoincidentes;
                }
            }

            return (mejorPuntuacion, mejorAmenidadesCoincidentes);
        }

        private List<string> ObtenerCoincidenciasDetalladas(ICollection<Domain.Entities.Preferencias> preferencias, Domain.Entities.Inmueble propiedad)
        {
            var coincidencias = new List<string>();

            foreach (var pref in preferencias)
            {
                // Se repite la lógica de filtro estricto aquí para asegurar que solo se detallan coincidencias válidas.
                // Se recomienda optimizar esta repetición en un helper si el código crece más.

                // Cláusulas de Guardia (repetidas para coherencia de detalle)
                if (!string.IsNullOrEmpty(pref.Tipo) && (propiedad.Tipos?.Trim().ToUpper() != pref.Tipo.Trim().ToUpper())) continue;
                if (!string.IsNullOrEmpty(pref.Operacion))
                {
                    var opPrefNorm = NormalizarOperacion(pref.Operacion);
                    var opPropNorm = NormalizarOperacion(propiedad.Operaciones);
                    if (opPropNorm != opPrefNorm) continue;
                }
                if (pref.Habitaciones.HasValue && pref.Habitaciones.Value > 0 && propiedad.Habitaciones < pref.Habitaciones.Value) continue;
                if (pref.Banos.HasValue && pref.Banos.Value > 0 && propiedad.Banos < pref.Banos.Value) continue;

                // Precio: aplicar solo si alguno de los límites fue realmente especificado (>0)
                decimal? prefMin = (pref.PrecioMin.HasValue && pref.PrecioMin.Value > 0) ? pref.PrecioMin : null;
                decimal? prefMax = (pref.PrecioMax.HasValue && pref.PrecioMax.Value > 0) ? pref.PrecioMax : null;

                if (prefMin.HasValue && prefMax.HasValue)
                {
                    if (propiedad.Precio < prefMin.Value || propiedad.Precio > prefMax.Value) continue;
                }
                else if (prefMin.HasValue)
                {
                    if (propiedad.Precio < prefMin.Value) continue;
                }
                else if (prefMax.HasValue)
                {
                    if (propiedad.Precio > prefMax.Value) continue;
                }

                // 1. Tipo
                if (!string.IsNullOrEmpty(pref.Tipo) && propiedad.Tipos?.Trim().ToUpper() == pref.Tipo.Trim().ToUpper())
                {
                    coincidencias.Add($"Tipo: {pref.Tipo} (Requisito estricto cumplido)");
                }

                // 2. Operación 
                if (!string.IsNullOrEmpty(pref.Operacion) && NormalizarOperacion(propiedad.Operaciones) == NormalizarOperacion(pref.Operacion))
                {
                    coincidencias.Add($"Operación: {pref.Operacion} (Requisito estricto cumplido)");
                }

                // 3. ✅ Precio (Se incluye el detalle SOLO si pasó el filtro estricto y el rango fue especificado)
                if (prefMin.HasValue || prefMax.HasValue)
                {
                    if (prefMin.HasValue && prefMax.HasValue)
                    {
                        if (propiedad.Precio >= prefMin.Value && propiedad.Precio <= prefMax.Value)
                        {
                            coincidencias.Add($"Precio: ${prefMin.Value:N0} - ${prefMax.Value:N0} (Rango estricto cumplido)");
                        }
                    }
                    else if (prefMin.HasValue)
                    {
                        if (propiedad.Precio >= prefMin.Value)
                        {
                            coincidencias.Add($"Precio: >= ${prefMin.Value:N0} (Límite inferior cumplido)");
                        }
                    }
                    else if (prefMax.HasValue)
                    {
                        if (propiedad.Precio <= prefMax.Value)
                        {
                            coincidencias.Add($"Precio: <= ${prefMax.Value:N0} (Límite superior cumplido)");
                        }
                    }
                }

                // 4. Habitaciones - DETALLE (Solo si cumple o supera el mínimo)
                if (!pref.Habitaciones.HasValue || propiedad.Habitaciones >= pref.Habitaciones.Value)
                {
                    if (pref.Habitaciones.HasValue && pref.Habitaciones.Value > 0)
                    {
                        string status;
                        if (propiedad.Habitaciones > pref.Habitaciones.Value + 2)
                        {
                            status = $" (Excede Deseado: {pref.Habitaciones.Value} con penalización)";
                        }
                        else
                        {
                            status = $" (Cumple Mínimo Deseado: {pref.Habitaciones.Value})";
                        }
                        coincidencias.Add($"Habitaciones: {propiedad.Habitaciones}{status}");
                    }
                    else
                    {
                        coincidencias.Add($"Habitaciones: {propiedad.Habitaciones}");
                    }
                }

                // 5. Baños - DETALLE (Solo si cumple o supera el mínimo)
                if (!pref.Banos.HasValue || propiedad.Banos >= pref.Banos.Value)
                {
                    if (pref.Banos.HasValue && pref.Banos.Value > 0)
                    {
                        string status;
                        if (propiedad.Banos > pref.Banos.Value + 2)
                        {
                            status = $" (Excede Deseado: {pref.Banos.Value} con penalización)";
                        }
                        else
                        {
                            status = $" (Cumple Mínimo Deseado: {pref.Banos.Value})";
                        }
                        coincidencias.Add($"Baños: {propiedad.Banos}{status}");
                    }
                    else
                    {
                        coincidencias.Add($"Baños: {propiedad.Banos}");
                    }
                }

                // 6. Ubicación - DETALLE
                if (!string.IsNullOrEmpty(pref.Ubicacion) && !string.IsNullOrEmpty(propiedad.Ubicaciones))
                {
                    var ubicacionesPreferidas = pref.Ubicacion.Split(',')
                        .Select(u => u.Trim())
                        .ToList();

                    var matchUbicacion = ubicacionesPreferidas.FirstOrDefault(u =>
                        propiedad.Ubicaciones.Contains(u, StringComparison.OrdinalIgnoreCase));

                    if (matchUbicacion != null)
                    {
                        coincidencias.Add($"Ubicación: {matchUbicacion} (Zona requerida)");
                    }
                }

                // 7. Amenidades (contar coincidencias)
                if (pref.PreferenciaAmenidades?.Any() == true && propiedad.Amenidades?.Any() == true)
                {
                    var amenidadesCoincidentes = pref.PreferenciaAmenidades
                        .Count(pa => propiedad.Amenidades.Any(ia =>
                            ia.Nombre.Trim().Equals(pa.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)));

                    if (amenidadesCoincidentes > 0)
                    {
                        coincidencias.Add($"{amenidadesCoincidentes} amenidad(es) coincidente(s)");
                    }
                }
            }

            return coincidencias.Distinct().ToList();
        }
    }
}