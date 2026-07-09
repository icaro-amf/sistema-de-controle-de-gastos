using Sistema_de_Controles_de_Gastos.Models;

//classe onde ficam as assinaturas dos metodos que serao chamados no repositorio de Transacao.
namespace Sistema_de_Controles_de_Gastos.Repositories.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<List<TransacaoModel>> BuscarTodasTransacoes();
        Task<TransacaoModel> AdicionarNovaTransacao(TransacaoModel transacao);
    }
}
