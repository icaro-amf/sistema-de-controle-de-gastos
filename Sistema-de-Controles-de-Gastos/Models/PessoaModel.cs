namespace Sistema_de_Controles_de_Gastos.Models
{
    //Criacao do objeto que representa a pessoa, com atributos exigidos. Como extra, foi adicionado uma lista de transacoes, que representa todas as transacoes vinculads a uma pessoa.
    public class PessoaModel
    {
        public int Id { get; set; }// como e um projeto que nao escalonara tanto, preferi utilizar o tipo int a fim de facilitar a implementacao.
        public String Nome { get; set; } = String.Empty;
        public int Idade { get; set; }
        public List<TransacaoModel> Transacoes { get; set; } = new(); //Vincula a pessoa com suas transacoes oq possibilita o acesso a todas as transacoes de uma pessoa
    }
}
