using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Repositories
{
    // Implementação concreta de ITransacaoRepository. Responsável por toda a
    // comunicação com o banco de dados relacionada a Transacao.
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly SistemaControleGastosDbContext _dbContext;

        public TransacaoRepository(SistemaControleGastosDbContext sistemaControleGastosDbContext)
        {
            _dbContext = sistemaControleGastosDbContext;
        }

        // Lista todas as transações cadastradas no sistema, de todas as pessoas.
        public async Task<List<TransacaoModel>> BuscarTodasTransacoes()
        {
            return await _dbContext.Transacoes.ToListAsync();
        }

        /// Insere uma nova transação no banco. As validações de negócio já foram feitas
        /// antes, no TransacaoController, este método assume que a transação
        /// recebida já é válida e só cuida da persistência.
        public async Task<TransacaoModel> AdicionarNovaTransacao(TransacaoModel transacao)
        {
            await _dbContext.Transacoes.AddAsync(transacao);
            await _dbContext.SaveChangesAsync();

            return transacao;
        }
    }
}
