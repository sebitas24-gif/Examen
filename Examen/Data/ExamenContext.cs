using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneoModelos;

public class ExamenContext : DbContext
{
    public ExamenContext(DbContextOptions<ExamenContext> options)
        : base(options)
    {
    }

    // Tus tablas
    public DbSet<Torneo> Torneos { get; set; } = default!;
    public DbSet<RegistroEstadistica> RegistroEstadisticas { get; set; } = default!;
    public DbSet<Partido> Partidos { get; set; } = default!;
    public DbSet<Jugador> Jugadors { get; set; } = default!;
    public DbSet<Equipo> Equipos { get; set; } = default!;

    // CONFIGURACIÓN DE RELACIONES
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. REGLAS PARA PARTIDOS (Ya las tenías)
        // Evitar que al borrar un Equipo se borren los Partidos en cascada
        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoLocal)
            .WithMany()
            .HasForeignKey(p => p.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Partido>()
            .HasOne(p => p.EquipoVisitante)
            .WithMany()
            .HasForeignKey(p => p.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. NUEVAS REGLAS PARA ESTADÍSTICAS (Esto arregla tu error actual)
        // Evitar que al borrar un Partido o Jugador se borren las estadísticas en cascada
        modelBuilder.Entity<RegistroEstadistica>()
            .HasOne(re => re.Partido)
            .WithMany(p => p.Estadisticas)
            .HasForeignKey(re => re.PartidoId)
            .OnDelete(DeleteBehavior.Restrict); // <--- AQUÍ ESTÁ LA SOLUCIÓN

        modelBuilder.Entity<RegistroEstadistica>()
            .HasOne(re => re.Jugador)
            .WithMany()
            .HasForeignKey(re => re.JugadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}