using Sistema_de_Controles_de_Gastos.Enums;

namespace Sistema_de_Controles_de_Gastos.Models
{
    //Model para transacao de qualquer tipo associada a um objeto da classe Pessoa
    public class TransacaoModel
    {
        public int Id { get; set; }
        public String Descricao { get; set; } = String.Empty;
        public decimal Valor { get; set; }//tipo decimal escolhido para evitar erros de arredondamento do float. fonte: "www.macoratti.net/12/12/c_num1.htm"
        public TipoTransacao Tipo { get; set; }//Escolhi utilizar Enums para facilitar a identificacao do tipo de transacao nao so por parte do usuario, mas tambem para o dev que utilizara da aplicacao.
        public int PessoaId { get; set; }

    }
}