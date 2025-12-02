using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TorneoModelos; // Tus clases (Torneo, Equipo, Jugador, Partido, RegistroEstadistica)
using Librerria.API.Consumer; // Tu librería Crud<T>

namespace Torneos.Consola
{
    internal class Program
    {
        // URL BASE DE LA API DESPLEGADA EN RENDER
        static readonly string BaseUrl = "https://examen-0nrj.onrender.com/api/";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("   CLIENTE DE CONSOLA - SIMULACIÓN DE TORNEO  ");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            try
            {
                // Solo se llama al método principal
                SimularFlujoCompleto();
            }
            catch (Exception ex)
            {
                // El error 500 se capturará aquí
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n💀 ERROR CRÍTICO AL INICIAR EL FLUJO: {ex.Message}");
                Console.WriteLine("Asegúrate que la API en Render esté encendida y la base de datos migrada.");
                Console.ResetColor();
            }

            Console.WriteLine("\n--- FIN DE LA SIMULACIÓN ---");
            Console.WriteLine("Presiona Enter para cerrar...");
            Console.ReadLine();
        }

        private static void SimularFlujoCompleto()
        {
            // ==========================================
            // 1. CREAR TORNEO (El Contenedor Principal)
            // ==========================================
            Console.WriteLine("\n1. Creando Torneo 'Copa Consola Final'...");
            Crud<Torneo>.EndPoint = BaseUrl + "Torneos";

            var torneoCreado = Crud<Torneo>.Create(new Torneo
            {
                Nombre = "Copa Consola Final",
                Tipo = "Mixto",
                FechaInicio = DateTime.Now.AddDays(7),
                FechaFin = DateTime.Now.AddMonths(1)
            });
            Console.WriteLine($"✅ Torneo creado con ID: {torneoCreado.Id}");

            // Si el objeto es nulo, lanzamos un error para detener el flujo
            if (torneoCreado == null) throw new Exception("Error al crear Torneo.");


            // ==========================================
            // 2. INSCRIBIR EQUIPOS (Usan el ID del Torneo)
            // ==========================================
            Console.WriteLine("\n2. Inscribiendo Equipos (Leones vs Tigres)...");
            Crud<Equipo>.EndPoint = BaseUrl + "Equipos";

            // Equipo Local (Leones)
            var leones = Crud<Equipo>.Create(new Equipo
            {
                Nombre = "Leones FC",
                Grupo = "A",
                TorneoId = torneoCreado.Id
            });
            Console.WriteLine($"   -> Leones FC inscrito (ID: {leones.Id})");

            // Equipo Visitante (Tigres)
            var tigres = Crud<Equipo>.Create(new Equipo
            {
                Nombre = "Tigres FC",
                Grupo = "A",
                TorneoId = torneoCreado.Id
            });
            Console.WriteLine($"   -> Tigres FC inscrito (ID: {tigres.Id})");


            // ==========================================
            // 3. REGISTRAR JUGADOR (Usa el ID del Equipo)
            // ==========================================
            Console.WriteLine("\n3. Registrando Jugador Estrella (Messi)...");
            Crud<Jugador>.EndPoint = BaseUrl + "Jugadors"; // Nota: Usar el nombre de la ruta real

            var messi = Crud<Jugador>.Create(new Jugador
            {
                NombreCompleto = "Lionel Messi",
                NumCamiseta = 10,
                EquipoId = leones.Id
            });
            Console.WriteLine($"✅ Jugador ID: {messi.Id} registrado.");


            // ==========================================
            // 4. CREAR PARTIDO (Usa IDs de Torneo y Equipos)
            // ==========================================
            Console.WriteLine("\n4. Programando Partido...");
            Crud<Partido>.EndPoint = BaseUrl + "Partidos";

            var partido = Crud<Partido>.Create(new Partido
            {
                Grupo = "Jornada 1",
                FechaPartido = DateTime.Now.AddDays(8),
                TorneoId = torneoCreado.Id,
                EquipoLocalId = leones.Id,
                EquipoVisitanteId = tigres.Id
            });
            Console.WriteLine($"✅ Partido Creado (ID: {partido.Id})");


            // ==========================================
            // 5. REGISTRAR ESTADÍSTICA (GOL)
            // ==========================================
            Console.WriteLine("\n5. Registrando Gol de Messi...");
            Crud<RegistroEstadistica>.EndPoint = BaseUrl + "RegistroEstadisticas";

            var gol = Crud<RegistroEstadistica>.Create(new RegistroEstadistica
            {
                TipoEstadistica = 1, // 1 = Gol
                Cantidad = 1,
                JugadorId = messi.Id,
                PartidoId = partido.Id
            });

            Console.WriteLine($"✅ ¡FLUJO COMPLETO EXITOSO! Gol registrado.");
        }
    }
}