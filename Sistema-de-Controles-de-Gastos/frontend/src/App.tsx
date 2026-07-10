import { useEffect, useState, useCallback } from "react";
import { pessoasApi, transacoesApi, totaisApi } from "./api";
import type { Pessoa, Transacao, TotaisResponse } from "./types";
import PessoaForm from "./components/PessoaForm";
import TransacaoForm from "./components/TransacaoForm";
import TransacoesList from "./components/TransacoesList";
import Totais from "./components/Totais";

import "./App.css";

function App() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [totais, setTotais] = useState<TotaisResponse | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  // useCallback para manter a mesma referência de função entre renders,
  // evitando re-renderizações desnecessárias nos componentes filhos
  // que recebem carregarTudo como prop.
  const carregarTudo = useCallback(async () => {
    try {
      const [p, t, tot] = await Promise.all([
        pessoasApi.listar(),
        transacoesApi.listar(),
        totaisApi.consultar(),
      ]);
      setPessoas(p);
      setTransacoes(t);
      setTotais(tot);
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao carregar dados.");
    } finally {
      setCarregando(false);
    }
  }, []);

  // Flag para ignorar atualizações de estado se o componente
  // for desmontado antes da requisição retornar (evita memory leak).
  useEffect(() => {
    let cancelado = false;

    async function executar() {
      try {
        const [p, t, tot] = await Promise.all([
          pessoasApi.listar(),
          transacoesApi.listar(),
          totaisApi.consultar(),
        ]);
        if (!cancelado) {
          setPessoas(p);
          setTransacoes(t);
          setTotais(tot);
        }
      } catch (err) {
        if (!cancelado) {
          setErro(err instanceof Error ? err.message : "Erro ao carregar dados.");
        }
      } finally {
        if (!cancelado) {
          setCarregando(false);
        }
      }
    }

    executar();
    return () => {
      cancelado = true;
    };
  }, []);

  async function handleDeletar(id: number) {
    if (!window.confirm("Tem certeza que deseja remover esta pessoa e todas as suas transacoes?")) {
      return;
    }
    try {
      await pessoasApi.deletar(id);
      carregarTudo();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao remover pessoa.");
    }
  }

  if (carregando) {
    return (
      <div className="loading-container">
        <div className="spinner" />
      </div>
    );
  }

  return (
    <div>
      <header className="app-header">
        <h1>Controle de Gastos Residenciais</h1>
        <p>Gerencie receitas e despesas de cada pessoa</p>
      </header>

      {erro && (
        <div className="alert alert-error">
          <span>{erro}</span>
          <button className="alert-close" onClick={() => setErro(null)}>
            &times;
          </button>
        </div>
      )}

      <div className="card">
        <PessoaForm onPessoaCriada={carregarTudo} />
      </div>

      <div className="card">
        <h2>Pessoas cadastradas</h2>
        {pessoas.length === 0 ? (
          <div className="empty-state">
            <div className="empty-state-icon">&#128100;</div>
            <p>Nenhuma pessoa cadastrada ainda</p>
          </div>
        ) : (
          <ul className="pessoa-list">
            {pessoas.map((pessoa) => (
              <li key={pessoa.id} className="pessoa-item">
                <div className="pessoa-info">
                  <div className="pessoa-avatar">
                    {pessoa.nome.charAt(0).toUpperCase()}
                  </div>
                  <div>
                    <div className="pessoa-nome">{pessoa.nome}</div>
                    <div className="pessoa-idade">{pessoa.idade} anos</div>
                  </div>
                </div>
                <button className="btn btn-danger" onClick={() => handleDeletar(pessoa.id)}>
                  Remover
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>

      <div className="card">
        <TransacaoForm pessoas={pessoas} onTransacaoCriada={carregarTudo} />
      </div>

      <div className="card">
        <TransacoesList transacoes={transacoes} />
      </div>

      {totais && (
        <div className="card">
          <Totais totais={totais} />
        </div>
      )}
    </div>
  );
}

export default App;
