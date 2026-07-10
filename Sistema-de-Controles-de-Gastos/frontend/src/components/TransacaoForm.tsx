import { useState } from "react";
import { transacoesApi } from "../api";
import type { CriarTransacaoInput, Pessoa, TipoTransacao } from "../types";

interface Props {
  pessoas: Pessoa[];
  onTransacaoCriada: () => void;
}

function TransacaoForm({ pessoas, onTransacaoCriada }: Props) {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
  const [pessoaId, setPessoaId] = useState("");
  const [erro, setErro] = useState<string | null>(null);
  const [errosCampo, setErrosCampo] = useState<{
    descricao?: string;
    valor?: string;
    pessoaId?: string;
  }>({});
  const [enviando, setEnviando] = useState(false);

  function validar(): boolean {
    const novosErros: { descricao?: string; valor?: string; pessoaId?: string } = {};

    if (!descricao.trim()) {
      novosErros.descricao = "Descricao e obrigatoria";
    }

    const valorNum = Number(valor);
    if (!valor) {
      novosErros.valor = "Valor e obrigatorio";
    } else if (isNaN(valorNum) || valorNum <= 0) {
      novosErros.valor = "Valor deve ser maior que zero";
    }

    if (!pessoaId) {
      novosErros.pessoaId = "Selecione uma pessoa";
    }

    setErrosCampo(novosErros);
    return Object.keys(novosErros).length === 0;
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    if (!validar()) return;

    setEnviando(true);
    const dados: CriarTransacaoInput = {
      descricao: descricao.trim(),
      valor: Number(valor),
      tipo,
      pessoaId: Number(pessoaId),
    };

    try {
      await transacoesApi.criar(dados);
      setDescricao("");
      setValor("");
      setTipo("Despesa");
      setPessoaId("");
      setErrosCampo({});
      onTransacaoCriada();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao cadastrar transacao.");
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <h3>Cadastrar transacao</h3>

      {erro && (
        <div className="alert alert-error">
          <span>{erro}</span>
          <button type="button" className="alert-close" onClick={() => setErro(null)}>
            &times;
          </button>
        </div>
      )}

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="pessoa">Pessoa</label>
          <select
            id="pessoa"
            className={`select ${errosCampo.pessoaId ? "input-error" : ""}`}
            value={pessoaId}
            onChange={(e) => {
              setPessoaId(e.target.value);
              if (errosCampo.pessoaId) setErrosCampo((prev) => ({ ...prev, pessoaId: undefined }));
            }}
          >
            <option value="">Selecione a pessoa</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome} ({p.idade} anos)
              </option>
            ))}
          </select>
          {errosCampo.pessoaId && <span className="error-text">{errosCampo.pessoaId}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="descricao">Descricao</label>
          <input
            id="descricao"
            type="text"
            className={`input ${errosCampo.descricao ? "input-error" : ""}`}
            placeholder="Descricao da transacao"
            value={descricao}
            onChange={(e) => {
              setDescricao(e.target.value);
              if (errosCampo.descricao) setErrosCampo((prev) => ({ ...prev, descricao: undefined }));
            }}
          />
          {errosCampo.descricao && <span className="error-text">{errosCampo.descricao}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="valor">Valor (R$)</label>
          <input
            id="valor"
            type="number"
            step="0.01"
            min="0.01"
            className={`input ${errosCampo.valor ? "input-error" : ""}`}
            placeholder="0,00"
            value={valor}
            onChange={(e) => {
              setValor(e.target.value);
              if (errosCampo.valor) setErrosCampo((prev) => ({ ...prev, valor: undefined }));
            }}
          />
          {errosCampo.valor && <span className="error-text">{errosCampo.valor}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="tipo">Tipo</label>
          <select
            id="tipo"
            className="select"
            value={tipo}
            onChange={(e) => setTipo(e.target.value as TipoTransacao)}
          >
            <option value="Despesa">Despesa</option>
            <option value="Receita">Receita</option>
          </select>
        </div>

        <div className="form-group form-group-action">
          <button type="submit" className="btn btn-primary" disabled={enviando}>
            {enviando ? "Cadastrando..." : "Adicionar"}
          </button>
        </div>
      </div>
    </form>
  );
}

export default TransacaoForm;
