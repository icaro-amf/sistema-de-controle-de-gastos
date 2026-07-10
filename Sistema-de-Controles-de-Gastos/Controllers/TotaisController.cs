using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_Controles_de_Gastos.Data;
using Sistema_de_Controles_de_Gastos.Enums;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotaisController : ControllerBase
    {
        private readonly SistemaControleGastosDbContext _dbContext;
        public TotaisController(SistemaControleGastosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> ObterTotais()
        {
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