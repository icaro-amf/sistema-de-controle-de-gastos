using Microsoft.AspNetCore.Mvc;
using Sistema_de_Controles_de_Gastos.Enums;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    //Endpoints de gerenciamento de transações (receitas e despesas).
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private readonly ITransacaoRepository _transacaoRepository;
        // Precisamos também do repositório de Pessoa aqui, mesmo sendo o
        // TransacaoController: é dele que vem a validação "essa pessoa existe?"
        // e o dado da idade, usado na regra do menor de idade.
        private readonly IPessoaRepository _pessoaRepository;

        public TransacaoController(
               ITransacaoRepository transacaoRepository,
               IPessoaRepository pessoaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _pessoaRepository = pessoaRepository;
        }

        // GET /api/transacao — Lista todas as transações cadastradas no sistema.
        [HttpGet]
        public async Task<ActionResult<List<TransacaoModel>>> BuscarTodasTransacoes()
        {
            var transacoes = await _transacaoRepository.BuscarTodasTransacoes();
            return Ok(transacoes);
        }

        // POST /api/transacao — Cadastra uma nova transação (receita ou despesa).
        /// Regras de negócio aplicadas nesta ação, na ordem em que são checadas:
        /// 1) Descrição e valor são obrigatórios/válidos (validação básica de formato).
        /// 2) A pessoa informada em PessoaId precisa existir previamente no cadastro
        ///    — não é permitido criar uma transação sem dono.
        /// 3) Se a pessoa for menor de idade (Idade < 18), somente transações do
        ///    tipo Despesa podem ser cadastradas para ela; tentativas de cadastrar
        ///    Receita para um menor são rejeitadas.
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