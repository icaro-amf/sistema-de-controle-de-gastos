using Microsoft.AspNetCore.Mvc;
using Sistema_de_Controles_de_Gastos.Enums;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRepository _pessoaRepository;

        public TransacaoController(
               ITransacaoRepository transacaoRepository,
               IPessoaRepository pessoaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _pessoaRepository = pessoaRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<TransacaoModel>>> BuscarTodasTransacoes()
        {
            var transacoes = await _transacaoRepository.BuscarTodasTransacoes();
            return Ok(transacoes);
        }

        [HttpPost]
        public async Task<ActionResult<TransacaoModel>> AdicionarNovaTransacao(TransacaoModel transacao)
        {
            if (string.IsNullOrWhiteSpace(transacao.Descricao))
                return BadRequest("A descrição é obrigatória.");

            if (transacao.Valor <= 0)
                return BadRequest("O valor deve ser maior que zero.");

            // Regra: a pessoa referenciada precisa existir no cadastro.
            var pessoa = await _pessoaRepository.BuscarPessoaPorId(transacao.PessoaId);
            if (pessoa is null)
                return BadRequest($"Pessoa com Id {transacao.PessoaId} não encontrada.");

            // Regra: menores de 18 anos só podem cadastrar despesas.
            if (pessoa.Idade < 18 && transacao.Tipo == TipoTransacao.Receita)
                return BadRequest($"{pessoa.Nome} é menor de idade. Apenas despesas podem ser cadastradas.");

            var transacaoCriada = await _transacaoRepository.AdicionarNovaTransacao(transacao);
            return Ok(transacaoCriada);
        }

    }
}