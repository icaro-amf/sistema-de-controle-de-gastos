namespace Sistema_de_Controles_de_Gastos.Models
{
    public class PessoaModel
    {
        public int Id { get; set; }
        public String Nome { get; set; } = String.Empty;
        public int Idade { get; set; }
        public List<TransacaoModel> Transacoes { get; set; } = new(); //Vincula a pessoa com suas transacoes oq possibilita o acesso a todas as transacoes de uma pessoa
    }
}
