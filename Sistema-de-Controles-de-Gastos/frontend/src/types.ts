// Espelha o enum TipoTransacao do back-end.
// Como a conversao ja foi feita no back, inves de usar um enum, ja utilizamos a string
export type TipoTransacao = "Receita" | "Despesa";

export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
  transacoes: Transacao[];
}

export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

export interface CriarPessoaInput {
  nome: string;
  idade: number;
}

export interface CriarTransacaoInput {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

export interface TotalPessoa {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotaisResponse {
  totaisPorPessoa: TotalPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoGeral: number;
}