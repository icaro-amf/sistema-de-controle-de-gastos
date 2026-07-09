using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Controles_de_Gastos.Models;
using Sistema_de_Controles_de_Gastos.Repositories.Interfaces;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaController(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PessoaModel>>> BuscarTodasPessoas()
        {
            var pessoas = await _pessoaRepository.BuscarTodasPessoas();
            return Ok(pessoas);
        }

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            var pessoa = await _pessoaRepository.BuscarPessoaPorId(id);

            if (pessoa is null)
            {
                return NotFound($"Pessoa com Id {id} não encontrada.");
            }

            await _pessoaRepository.DeletarPessoa(id);
            return Ok($"Pessoa {id} removida com sucesso.");
        }
    }
} 

