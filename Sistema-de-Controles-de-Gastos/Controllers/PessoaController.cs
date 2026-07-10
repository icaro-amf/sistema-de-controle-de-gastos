using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    //Endpoints de gerenciamento de pessoas: criação, listagem e deleção.
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaController(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        //GET /api/pessoas — Lista todas as pessoas cadastradas.
        [HttpGet]
        public async Task<ActionResult<List<PessoaModel>>> BuscarTodasPessoas()
        {
            var pessoas = await _pessoaRepository.BuscarTodasPessoas();
            return Ok(pessoas);
        }

        //Endpoint extra - GET /api/pessoas/{id} — Lista pessoas com id específico.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PessoaModel>> BuscarPessoaPorId(int id)
        {
            var pessoa = await _pessoaRepository.BuscarPessoaPorId(id);

            if (pessoa is null)
            {
                return NotFound($"Pessoa com Id {id} não encontrada.");
            }

            return Ok(pessoa);
        }

        //POST /api/pessoas — Cadastra uma nova pessoa.
        [HttpPost]
        public async Task<ActionResult<PessoaModel>> AdicionarNovaPessoa(PessoaModel pessoa)
        {
            if (string.IsNullOrWhiteSpace(pessoa.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            }

            if (pessoa.Idade < 0)
            {
                return BadRequest("A idade não pode ser negativa.");
            }

            var pessoaCriada = await _pessoaRepository.AdicionarNovaPessoa(pessoa);

            return Ok(pessoaCriada);
        }

        //DELETE /api/pessoas/{id} — Remove uma pessoa do sistema.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            var pessoa = await _pessoaRepository.BuscarPessoaPorId(id);

            if (pessoa is null)
            {
                return NotFound($"Pessoa com Id {id} não encontrada.");
            }

            await _pessoaRepository.DeletarPessoa(id);
            return NoContent();//troca para NoContent (204) facilitar comunicacao com o front
        }
    }
} 

