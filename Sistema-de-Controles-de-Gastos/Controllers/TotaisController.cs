using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Enums;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    // Endpoint de consulta de totais financeiros.
    [Route("api/[controller]")]
    [ApiController]
    public class TotaisController : ControllerBase
    {
        //o DbContext é injetado e consultado diretamente neste controller.
        private readonly SistemaControleGastosDbContext _dbContext;
        public TotaisController(SistemaControleGastosDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET /api/totais — Retorna, para cada pessoa cadastrada, o total de receitas, o total de despesas e o saldo (receitas - despesas).
        [HttpGet]
        public async Task<ActionResult> ObterTotais()
        {
            // Include(p => p.Transacoes): sem isso, o EF Core traria cada Pessoa com a lista de Transacoes vazia, mesmo que existam transações dela no banco.
            //Assim como em PessoaRepository, se eu deixasse sem o include me retornaria apenas a classe instanciada no objeto de Pessoa.
            var pessoas = await _dbContext.Pessoas.Include(p => p.Transacoes).ToListAsync();
            var totaisPorPessoa = pessoas.Select(p =>
            {
                var totalReceitas = p.Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
                var totalDespesas = p.Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);

                return new
                {
                    PessoaId = p.Id,
                    Nome = p.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            }).ToList();

            var totalGeralReceitas = totaisPorPessoa.Sum(p => p.TotalReceitas);
            var totalGeralDespesas = totaisPorPessoa.Sum(p => p.TotalDespesas);

            var resultado = new
            {
                TotaisPorPessoa = totaisPorPessoa,
                TotalGeralReceitas = totalGeralReceitas,
                TotalGeralDespesas = totalGeralDespesas,
                SaldoGeral = totalGeralReceitas - totalGeralDespesas
            };
            
            return Ok(resultado);
        }
    }
}