using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Models;

//Configuracao gerais e de tabelas do bd.
namespace Sistema_de_Controles_de_Gastos.Data
{
    public class SistemaControleGastosDbContext : DbContext
    {
        public SistemaControleGastosDbContext(DbContextOptions<SistemaControleGastosDbContext> options) : base(options)
        {
        }

        //cria a tabela Pessoas e Transacoes no bd
        public DbSet<PessoaModel> Pessoas { get; set; }
        public DbSet<TransacaoModel> Transacoes { get; set; }

        //Cada transacao esta linkada a uma pessoa, se a pessoa for deletada, todas as transacoes dela tambem serao deletadas.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransacaoModel>()
               .HasOne<PessoaModel>()
               .WithMany(p => p.Transacoes)
               .HasForeignKey(t => t.PessoaId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
