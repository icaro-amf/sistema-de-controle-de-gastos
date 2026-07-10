using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Models;

//Configuracao gerais e de tabelas do bd.
namespace Sistema_de_Controles_de_Gastos.Data
{
    /// Contexto do Entity Framework Core responsável pelo acesso ao banco de dados SQLite. 
    /// Representa a "ponte" entre as classes no C#, como PessoaModel e TransacaoModel, com as tabelas reais do banco (Pessoas, Transacoes).
    /// O SQLite grava os dados em um arquivo local (gastos.db), o que garante que as informações persistam mesmo após o encerramento da aplicação.
    public class SistemaControleGastosDbContext : DbContext
    {
        public SistemaControleGastosDbContext(DbContextOptions<SistemaControleGastosDbContext> options) : base(options)
        {
        }

        //Cada DbSet<T> representa uma tabela no banco. O EF Core cria a tabela a partir de PessoaModel e TransacaoModel
        public DbSet<PessoaModel> Pessoas { get; set; }
        public DbSet<TransacaoModel> Transacoes { get; set; }

        //Cada transacao esta linkada a uma pessoa, se a pessoa for deletada, todas as transacoes dela tambem serao deletadas.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Uma Transacao pertence a uma Pessoa (relação N:1), ligada pela coluna PessoaId.
            modelBuilder.Entity<TransacaoModel>()
               .HasOne<PessoaModel>()
               .WithMany(p => p.Transacoes)
               .HasForeignKey(t => t.PessoaId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
