import { useState } from "react";
import { pessoasApi } from "../api";
import type { CriarPessoaInput } from "../types";

interface Props {
  onPessoaCriada: () => void;
}

function PessoaForm({ onPessoaCriada }: Props) {
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState("");
  const [erro, setErro] = useState<string | null>(null);
  const [errosCampo, setErrosCampo] = useState<{ nome?: string; idade?: string }>({});
  const [enviando, setEnviando] = useState(false);

  function validar(): boolean {
    const novosErros: { nome?: string; idade?: string } = {};

    if (!nome.trim()) {
      novosErros.nome = "Nome e obrigatorio";
    } else if (nome.trim().length < 2) {
      novosErros.nome = "Nome deve ter pelo menos 2 caracteres";
    }

    const idadeNum = Number(idade);
    if (!idade) {
      novosErros.idade = "Idade e obrigatoria";
    } else if (isNaN(idadeNum) || idadeNum <= 0) {
      novosErros.idade = "Idade deve ser um numero positivo";
    } else if (idadeNum > 150) {
      novosErros.idade = "Idade invalida";
    }

    setErrosCampo(novosErros);
    return Object.keys(novosErros).length === 0;
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErro(null);

    if (!validar()) return;

    setEnviando(true);
    const dados: CriarPessoaInput = {
      nome: nome.trim(),
      idade: Number(idade),
    };

    try {
      await pessoasApi.criar(dados);
      setNome("");
      setIdade("");
      setErrosCampo({});
      onPessoaCriada();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao cadastrar pessoa.");
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <h3>Cadastrar pessoa</h3>

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
          <label htmlFor="nome">Nome</label>
          <input
            id="nome"
            type="text"
            className={`input ${errosCampo.nome ? "input-error" : ""}`}
            placeholder="Nome da pessoa"
            value={nome}
            onChange={(e) => {
              setNome(e.target.value);
              if (errosCampo.nome) setErrosCampo((prev) => ({ ...prev, nome: undefined }));
            }}
          />
          {errosCampo.nome && <span className="error-text">{errosCampo.nome}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="idade">Idade</label>
          <input
            id="idade"
            type="number"
            className={`input ${errosCampo.idade ? "input-error" : ""}`}
            placeholder="Idade"
            value={idade}
            onChange={(e) => {
              setIdade(e.target.value);
              if (errosCampo.idade) setErrosCampo((prev) => ({ ...prev, idade: undefined }));
            }}
          />
          {errosCampo.idade && <span className="error-text">{errosCampo.idade}</span>}
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

export default PessoaForm;
