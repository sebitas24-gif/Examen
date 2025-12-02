using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TorneoModelos; // Asegúrate de que esta referencia esté agregada en Dependencias

namespace Torneos.ConsolaApp
{
    class Program
    {
        // ⚠️ TU URL DE RENDER (Sin "/swagger" al final)
        // Asegúrate de que esta sea la correcta
        static string baseUrl = "https://examen-0nrj.onrender.com";

        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri(baseUrl);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("   CLIENTE DE CONSOLA - GESTIÓN DE TORNEOS    ");
            Console.WriteLine("==============================================");
            Console.ResetColor();
            Console.WriteLine($"\n📡 Conectando a: {baseUrl} ...\n");

            try
            {
                // ---------------------------------------------------------
                // PASO 1: CREAR EL TORNEO
                // ---------------------------------------------------------
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1. Creando la 'Copa Final 2025'...");
                Console.ResetColor();

                var nuevoTorneo = new Torneo
                {
                    // No enviamos ID, la BD lo crea
                    Nombre = "Copa Final 2025",
                    Tipo = "Mixto",
                    FechaInicio = DateTime.Now.AddDays(7),
                    FechaFin = DateTime.Now.AddMonths(1)
                    // Equipos y Partidos se envían vacíos o nulos al crear
                };

                // Enviamos POST a /api/Torneos
                var respTorneo = await client.PostAsJsonAsync("api/Torneos", nuevoTorneo);

                if (!respTorneo.IsSuccessStatusCode)
                {
                    string error = await respTorneo.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear torneo ({respTorneo.StatusCode}): {error}");
                }

                var torneoDb = await respTorneo.Content.ReadFromJsonAsync<Torneo>();
                Console.WriteLine($"✅ Torneo creado con ID: {torneoDb!.Id}\n");


                // ---------------------------------------------------------
                // PASO 2: INSCRIBIR EQUIPOS
                // ---------------------------------------------------------
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("2. Inscribiendo Equipos...");
                Console.ResetColor();

                // --- Equipo 1: Local ---
                var equipo1 = new Equipo
                {
                    Nombre = "Leones FC",
                    Grupo = "A",
                    TorneoId = torneoDb.Id
                };
                var rEq1 = await client.PostAsJsonAsync("api/Equipos", equipo1);
                rEq1.EnsureSuccessStatusCode();
                var leones = await rEq1.Content.ReadFromJsonAsync<Equipo>();
                Console.WriteLine($"   -> Equipo 1 inscrito: {leones!.Nombre} (ID: {leones.Id})");

                // --- Equipo 2: Visitante ---
                var equipo2 = new Equipo
                {
                    Nombre = "Tigres FC",
                    Grupo = "A",
                    TorneoId = torneoDb.Id
                };
                var rEq2 = await client.PostAsJsonAsync("api/Equipos", equipo2);
                rEq2.EnsureSuccessStatusCode();
                var tigres = await rEq2.Content.ReadFromJsonAsync<Equipo>();
                Console.WriteLine($"   -> Equipo 2 inscrito: {tigres!.Nombre} (ID: {tigres.Id})\n");


                // ---------------------------------------------------------
                // PASO 3: REGISTRAR JUGADOR
                // ---------------------------------------------------------
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("3. Registrando Jugador...");
                Console.ResetColor();

                var jugador = new Jugador
                {
                    NombreCompleto = "Lionel Messi",
                    NumCamiseta = 10,
                    EquipoId = leones.Id // Pertenece a Leones FC
                };

                var rJug = await client.PostAsJsonAsync("api/Jugadores", jugador);
                rJug.EnsureSuccessStatusCode();
                var messi = await rJug.Content.ReadFromJsonAsync<Jugador>();
                Console.WriteLine($"✅ Jugador registrado: {messi!.NombreCompleto} (ID: {messi.Id})\n");


                // ---------------------------------------------------------
                // PASO 4: CREAR PARTIDO
                // ---------------------------------------------------------
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("4. Programando Partido...");
                Console.ResetColor();

                // Basado en tu JSON, asegúrate de que tu modelo Partido tenga estas propiedades
                var partido = new Partido
                {
                    Grupo = "Jornada 1",
                    FechaPartido = DateTime.Now.AddDays(8),
                    TorneoId = torneoDb.Id,
                    EquipoLocalId = leones.Id,
                    EquipoVisitanteId = tigres.Id // ⚠️ IMPORTANTE: Si quitaste esto del modelo, bórralo aquí
                };

                var rPartido = await client.PostAsJsonAsync("api/Partidos", partido);

                if (rPartido.IsSuccessStatusCode)
                {
                    var partidoDb = await rPartido.Content.ReadFromJsonAsync<Partido>();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Partido Creado (ID: {partidoDb!.Id})");
                    Console.WriteLine($"   Fecha: {partidoDb.FechaPartido}");
                    Console.ResetColor();

                    // ---------------------------------------------------------
                    // PASO 5: REGISTRAR ESTADÍSTICA (GOL)
                    // ---------------------------------------------------------
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n5. Registrando un Gol...");
                    Console.ResetColor();

                    var gol = new RegistroEstadistica
                    {
                        TipoEstadistica = 1, // 1 = Gol
                        Cantidad = 1,
                        JugadorId = messi.Id,      // Messi anotó
                        PartidoId = partidoDb.Id   // En este partido
                    };

                    var rStat = await client.PostAsJsonAsync("api/RegistroEstadisticas", gol);

                    if (rStat.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ ¡GOL REGISTRADO EN LA NUBE! Messi anotó en el partido {partidoDb.Id}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Error al registrar gol: {rStat.StatusCode}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Error al crear partido: {rPartido.StatusCode}");
                    // Leemos el error detallado de la API
                    string errorPart = await rPartido.Content.ReadAsStringAsync();
                    Console.WriteLine($"   Detalle: {errorPart}");
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n💀 ERROR FATAL: {ex.Message}");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("Posibles causas:");
                Console.WriteLine("1. La URL de Render está mal escrita.");
                Console.WriteLine("2. La API en Render está apagada (entra al link en el navegador para despertarla).");
                Console.WriteLine("3. La base de datos (Postgres) no está conectada.");
            }

            Console.ResetColor();
            Console.WriteLine("\n----------------------------------------------");
            Console.WriteLine("Presiona Enter para cerrar...");
            Console.ReadLine();
        }
    }
}