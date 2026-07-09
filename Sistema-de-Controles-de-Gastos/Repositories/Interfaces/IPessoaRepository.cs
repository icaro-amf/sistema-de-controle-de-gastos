using Sistema_de_Controles_de_Gastos.Models;

//classe onde ficam as assinaturas dos metodos que serao chamados no repositorio de Pessoa.
namespace Sistema_de_Controles_de_Gastos.Repositories.Interfaces
{
    public interface IPessoaRepository
    {
        Task<List<PessoaModel>> BuscarTodasPessoas();
        Task<PessoaModel> BuscarPessoaPorId(int id);
        Task<PessoaModel> AdicionarNovaPessoa(PessoaModel pessoa);
        Task<bool> DeletarPessoa(int id); // lembrar que ao deletar, todo historico vai junto
    }
}
