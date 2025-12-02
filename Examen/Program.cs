using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; // NECESARIO para el logger en el try/catch
using Newtonsoft.Json.Serialization;
using Npgsql; // Necesario para AppContext.SetSwitch

namespace Examen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // --- FIX 1: ARREGLO DE FECHAS PARA POSTGRES ---
            // Esto es crucial para evitar el error al guardar DateTime.Now en Postgres
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // ----------------------------------------------------

            var builder = WebApplication.CreateBuilder(args);

            // 1. CONFIGURACIÓN DE BASE DE DATOS (POSTGRES)
            // La conexión busca "ExamenContext"
            var connectionString = builder.Configuration.GetConnectionString("ExamenContext")
                ?? throw new InvalidOperationException("Connection string 'ExamenContext' not found in configuration.");

            builder.Services.AddDbContext<ExamenContext>(options =>
                options.UseNpgsql(connectionString));


            // 2. REGISTRO DE CONTROLADORES Y JSON (Consolidado)
            // Se realiza en una sola llamada para evitar duplicados y registrar Newtonsoft
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });


            // --- OTROS SERVICIOS ---
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // -----------------------


            var app = builder.Build();

            // ====================================================
            // BLOQUE DE MIGRACIÓN AUTOMÁTICA (MANTENER AQUÍ)
            // ====================================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ExamenContext>();
                    context.Database.Migrate(); // <-- EJECUTA LAS MIGRACIONES EN POSTGRES
                }
                catch (Exception ex)
                {
                    // Esto registrará el error de conexión si falla en Render
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error al migrar o conectar la base de datos.");
                }
            }
            // ====================================================


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) // Usar esto para debug local
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Usar Swagger también en la nube para probar (aunque no esté en Development)
            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}