using Astuc.Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Globalization;


public class ProjectDbContext : IdentityDbContext<ApplicationUser>
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
     : base(options)
    {
    }
    //public DbSet<Evento> Eventos { get; set; } 
   
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<IdentityRole> IdentityRoles { get; set; }

    public DbSet<IdentityUserRole<string>> IdentityUserRoles { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; }
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<ImagenInmueble> ImagenesInmueble { get; set; }
    public DbSet<ImagenProyecto> ImagenesProyectos { get; set; }
    public DbSet<AmenidadInmueble> AmenidadesInmueble { get; set; }
    public DbSet<AmenidadProyecto> AmenidadesProyecto { get; set; }
    public DbSet<Preferencias> Preferencias { get; set; }
    public DbSet<PreferenciaAmenidad> PreferenciasAmenidades { get; set; } 
    public DbSet<Interaccion> Interacciones { get; set; }
    public DbSet<Domain.Entities.Match> Matches { get; set; }

    public DbSet<ApplicationUser> Usuarios { get; set; }

    public DbSet<CertificacionAgente> CertificacionesAgentes { get; set; }

    public DbSet<Domain.Entities.PreferenciaAmenidad> PreferenciaAmenidades { get; set; }

    public DbSet<Amenidad> Amenidades { get; set; }

    public DbSet<Zona> Zonas { get; set; }

    public DbSet<Articulo> Articulos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });


        modelBuilder.Entity<Inmueble>(entity =>
        {
            entity.Property(i => i.Precio).HasColumnType("decimal(18,2)");
            entity.Property(i => i.MetrosCuadrados).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<ImagenInmueble>(entity =>
        {
            entity.HasOne(ii => ii.Inmueble)
                 .WithMany(i => i.ImagenesReferencia)
                 .HasForeignKey(ii => ii.InmuebleId)
                 .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AmenidadInmueble>(entity =>
        {
            entity.HasOne(ai => ai.Inmueble)
                 .WithMany(i => i.Amenidades)
                 .HasForeignKey(ai => ai.InmuebleId)
                 .OnDelete(DeleteBehavior.Cascade);
        });

       


        // Configuración de la relación muchos-a-muchos
        //modelBuilder.Entity<PreferenciaAmenidad>()
        //    .HasKey(pa => new { pa.PreferenciasId});

        //modelBuilder.Entity<PreferenciaAmenidad>()
        //    .HasOne(pa => pa.Preferencias)
        //    .WithMany(p => p.PreferenciaAmenidades)
        //    .HasForeignKey(pa => pa.PreferenciasId);

        // Configura el nombre exacto de la tabla
        modelBuilder.Entity<PreferenciaAmenidad>()
            .ToTable("PreferenciaAmenidad");

        // Configuración de relaciones
        modelBuilder.Entity<Preferencias>()
    .HasOne(p => p.Cliente)
    .WithMany(c => c.Preferencias)
    .HasForeignKey(p => p.ClienteId)
    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Domain.Entities.Match>()
       .HasOne(m => m.Cliente)
       .WithMany(c => c.Matches)
       .HasForeignKey(m => m.ClienteId)
       .OnDelete(DeleteBehavior.Cascade);



    }
    }







