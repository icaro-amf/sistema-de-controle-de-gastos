using Sistema_de_Controles_de_Gastos.Enums;

namespace Sistema_de_Controles_de_Gastos.Models
{
    //Model para transacao de qualquer tipo associada a um objeto da classe Pessoa
    public class TransacaoModel
    {
        public int Id { get; set; }
        public String Descricao { get; set; } = String.Empty;
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }//Agora utiliza enums para possibilitar escalonamento e facilitar identificacao do tipo
        public int PessoaId { get; set; }

    }
}