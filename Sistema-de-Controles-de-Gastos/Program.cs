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

            //Metodo para adicionar o enumeração como string no JSON da requisição e resposta da API.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions
                .Converters
                .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services
                .AddDbContext<SistemaControleGastosDbContext>(
                    options => options.UseSqlite(builder.Configuration.GetConnectionString("Database"))
                );

            // Registra os repositórios: sempre que um Controller pedir
            // IPessoaRepository/ITransacaoRepository no construtor, o ASP.NET
            // Core vai injetar PessoaRepository/TransacaoRepository automaticamente.
            // AddScoped = uma instância nova por requisição HTTP.
            builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
            builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

            const string PoliticaFrontEnd = "FrontEnd";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(PoliticaFrontEnd, policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SistemaControleGastosDbContext>();
                db.Database.EnsureCreated();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors(PoliticaFrontEnd);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
