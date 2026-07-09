namespace Sistema_de_Controles_de_Gastos.Models
{
    public class TransacaoModel
    {
        public int TransacaoId { get; set; }//msm questao que na classe Pessoa
        public String Descricao { get; set; } = String.Empty;
        public decimal Valor { get; set; }//verificar se o tipo decimal é o mais adequado para representar valores monetários.
        public int Receita { get; set; }//Logica: Se Receita = true, valor entrando. Senao, valor saindo(despesa). Escolhe bool inves de Boolean, nao pode ter retorno nulo.
        public int PessoaId { get; set; }
    }
}