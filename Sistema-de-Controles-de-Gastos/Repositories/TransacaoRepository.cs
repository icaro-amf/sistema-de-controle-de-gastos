using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly SistemaControleGastosDbContext _dbContext;

        public TransacaoRepository(SistemaControleGastosDbContext sistemaControleGastosDbContext)
        {
            _dbContext = sistemaControleGastosDbContext;
        }
        public async Task<List<TransacaoModel>> BuscarTodasTransacoes()
        {
            return await _dbContext.Transacoes.ToListAsync();
        }

        public async Task<TransacaoModel> AdicionarNovaTransacao(TransacaoModel transacao)
        {
            await _dbContext.Transacoes.AddAsync(transacao);
            await _dbContext.SaveChangesAsync();

            return transacao;
        }
    }
}
