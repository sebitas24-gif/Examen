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

        public DbSet<TorneoModelos.Equipo> Equipo { get; set; } = default!;

public DbSet<TorneoModelos.Jugador> Jugador { get; set; } = default!;

public DbSet<TorneoModelos.Partido> Partido { get; set; } = default!;

public DbSet<TorneoModelos.RegistroEstadistica> RegistroEstadistica { get; set; } = default!;

public DbSet<TorneoModelos.Torneo> Torneo { get; set; } = default!;
    }
