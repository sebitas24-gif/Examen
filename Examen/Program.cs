using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Examen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ExamenContext>(options =>
               // options.UseSqlServer(builder.Configuration.GetConnectionString("ExamenContext") ?? throw new InvalidOperationException("Connection string 'ExamenContext' not found.")));
           //builder.Services.AddDbContext<ExamenContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("ExamenContext") ?? throw new InvalidOperationException("Connection string 'ExamenContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services
               .AddControllers()
               .AddNewtonsoftJson(
              options =>
              options.SerializerSettings.ReferenceLoopHandling
              = Newtonsoft.Json.ReferenceLoopHandling.Ignore
          );

            var app = builder.Build();
            // ====================================================
            // BLOQUE DE MIGRACIÓN AUTOMÁTICA (Copia desde aquí)
            // ====================================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ExamenContext>();
                    context.Database.Migrate(); // <--- ESTO CREA LAS TABLAS EN POSTGRES
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error al migrar la base de datos.");
                }
            }
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
