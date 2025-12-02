using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TorneoModelos;

    public class ExamenContext : DbContext
    {
        public ExamenContext (DbContextOptions<ExamenContext> options)
            : base(options)
        {
        }

        public DbSet<TorneoModelos.Torneo> Torneos { get; set; } = default!;

public DbSet<TorneoModelos.RegistroEstadistica> RegistroEstadisticas { get; set; } = default!;

public DbSet<TorneoModelos.Partido> Partidos { get; set; } = default!;

public DbSet<TorneoModelos.Jugador> Jugadors { get; set; } = default!;

public DbSet<TorneoModelos.Equipo> Equipos { get; set; } = default!;
    }
