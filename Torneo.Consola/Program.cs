using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TorneoModelos;

namespace Torneos.ConsolaApp
{
    class Program
    {
        // ⚠️ ASEGÚRATE QUE TU API EN RENDER YA TENGA EL CÓDIGO NUEVO (HAZ PUSH PRIMERO)
        static string baseUrl = "https://examen-0nrj.onrender.com";

        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromMinutes(2); // Darle tiempo a Render

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================================");
            Console.WriteLine("   PRUEBA DE GENERACIÓN AUTOMÁTICA DE PARTIDOS");
            Console.WriteLine("==============================================");
            Console.ResetColor();

            try
            {
                // 1. CREAR TORNEO
                Console.WriteLine("\n1. Creando Torneo 'Liga Automática 2025'...");
                var torneo = new Torneo { Nombre = "Liga Automática 2025", Tipo = "Liga", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) };

                var respTorneo = await client.PostAsJsonAsync("api/Torneos", torneo);
                respTorneo.EnsureSuccessStatusCode();
                var torneoDb = await respTorneo.Content.ReadFromJsonAsync<Torneo>();
                Console.WriteLine($"✅ Torneo ID: {torneoDb!.Id}");

                // 2. INSCRIBIR 4 EQUIPOS (Requisito mínimo)
                Console.WriteLine("\n2. Inscribiendo 4 equipos...");
                string[] nombresEquipos = { "Real Madrid", "Barcelona", "Bayern", "Liverpool" };

                foreach (var nombre in nombresEquipos)
                {
                    var equipo = new Equipo { Nombre = nombre, Grupo = "X", TorneoId = torneoDb.Id };
                    await client.PostAsJsonAsync("api/Equipos", equipo);
                    Console.WriteLine($"   -> {nombre} inscrito.");
                }

                // 3. EL MOMENTO DE LA VERDAD: GENERAR CALENDARIO
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n3. ⚡ Solicitando a la API que genere el calendario...");
                Console.ResetColor();

                // Llamamos al nuevo endpoint que creaste
                var respIniciar = await client.PostAsync($"api/Torneos/{torneoDb.Id}/iniciar", null);

                if (respIniciar.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ ¡ÉXITO! La API ha generado los partidos automáticamente.");
                    Console.ResetColor();

                    // 4. VER LOS PARTIDOS CREADOS
                    Console.WriteLine("\n4. Consultando fixture generado...");
                    var respPartidos = await client.GetAsync($"api/Torneos/{torneoDb.Id}");
                    var torneoCompleto = await respPartidos.Content.ReadFromJsonAsync<Torneo>();

                    Console.WriteLine("--------------------------------");
                    foreach (var p in torneoCompleto!.Partidos!)
                    {
                        // Buscamos nombres (en una app real haríamos join, aquí simplificamos)
                        Console.WriteLine($"📅 Partido {p.Id}: Equipo {p.EquipoLocalId} vs Equipo {p.EquipoVisitanteId} (Grupo {p.Grupo})");
                    }
                    Console.WriteLine("--------------------------------");
                }
                else
                {
                    string error = await respIniciar.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ FALLÓ LA GENERACIÓN: {error}");
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine("\nPresiona Enter para salir...");
            Console.ReadLine();
        }
    }
}