using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Repositories;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;
namespace Sistema_de_Controles_de_Gastos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<SistemaControleGastosDbContext>(
                    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
                );

            builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
