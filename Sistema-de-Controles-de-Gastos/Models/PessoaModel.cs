namespace Sistema_de_Controles_de_Gastos.Models
{
    public class PessoaModel
    {
        public int PessoaId { get; set; } //verificar tipagem --> nao tenho ctz se devo me preocupar com o escalonamento.
        public String Nome { get; set; } = String.Empty;
        public int Idade { get; set; }
    }
}
