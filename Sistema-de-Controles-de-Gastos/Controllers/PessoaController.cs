using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_de_Controles_de_Gastos.Models;

namespace Sistema_de_Controles_de_Gastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PessoaModel>> BuscarTodosOsUsuarios()
        {
            return Ok();
        }
    }
} 

