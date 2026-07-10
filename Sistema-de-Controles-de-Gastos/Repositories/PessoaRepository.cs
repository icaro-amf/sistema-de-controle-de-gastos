using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Repositories
{
    /// Implementação concreta da interface IPessoaRepository. Responsável por toda a
    /// comunicação com o bd relacionada a Pessoa, isolando o PessoaController
    /// de detalhes de acesso a dados (EF Core, queries, etc.).
    public class PessoaRepository : IPessoaRepository
    {
        private readonly SistemaControleGastosDbContext _dbContext;
        public PessoaRepository(SistemaControleGastosDbContext sistemaControleGastosDbContext)
        {
            _dbContext = sistemaControleGastosDbContext;
        }

        /// Busca uma pessoa pelo Id, já trazendo suas transações junto (Include).
        /// Retorna null se nenhuma pessoa for encontrada com esse Id — quem chama
        /// este método é responsável por tratar esse caso (ver PessoaController
        /// e TransacaoController, que checam "is null" antes de prosseguir).
        public async Task<PessoaModel> BuscarPessoaPorId(int id)
        {
            return await _dbContext.Pessoas
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PessoaModel>> BuscarTodasPessoas()
        {
            return await _dbContext.Pessoas
                .Include(p => p.Transacoes)
                .ToListAsync();
        }

        /// Insere uma nova pessoa no banco. O Id não precisa ser informado:
        /// é gerado automaticamente e preenchido em "pessoa.Id" logo após o
        /// SaveChangesAsync, que é quando o EF Core efetivamente executa o
        /// INSERT e lê de volta o valor gerado pelo SQLite.
        public async Task<PessoaModel> AdicionarNovaPessoa(PessoaModel pessoa)
        {
            await _dbContext.Pessoas.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            return pessoa;
        }

        /// Remove uma pessoa do banco de dados.
        /// Regra de negócio: como o relacionamento Pessoa-Transacao está
        /// configurado com OnDelete(DeleteBehavior.Cascade) no DbContext,
        /// o próprio SQLite se encarrega de apagar todas as transações
        /// vinculadas a essa pessoa.
        /// Retorna false se a pessoa não existir (quem chama decide o que fazer
        /// com isso, normalmente retornando 404 no Controller).
        public async Task<bool> DeletarPessoa(int id)
        {
            PessoaModel pessoaPorId = await BuscarPessoaPorId(id);

            if(pessoaPorId == null)
            {
                throw new Exception($"Pessoa para o ID: {id} não foi encontrada na base de dados.");
            }

            _dbContext.Pessoas.Remove(pessoaPorId);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
