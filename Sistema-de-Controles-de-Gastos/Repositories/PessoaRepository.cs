using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly SistemaControleGastosDbContext _dbContext;
        public PessoaRepository(SistemaControleGastosDbContext sistemaControleGastosDbContext)
        {
            _dbContext = sistemaControleGastosDbContext;
        }
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

        public async Task<PessoaModel> AdicionarNovaPessoa(PessoaModel pessoa)
        {
            await _dbContext.Pessoas.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            return pessoa;
        }

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
