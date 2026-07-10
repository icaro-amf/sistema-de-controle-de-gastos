import type {
  Pessoa,
  CriarPessoaInput,
  Transacao,
  CriarTransacaoInput,
  TotaisResponse,
} from "./types";

const API_BASE_URL = "http://localhost:5000/api"; 

export class ApiError extends Error {}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (!response.ok) {
    const mensagem = await response.text();
    throw new ApiError(mensagem || `Erro na requisição (status ${response.status})`);
  }

  // Respostas 204 (No Content) não possuem corpo.
  // Cast como T para manter a assinatura de tipo da função.
  if (response.status === 204) return undefined as T;

  return (await response.json()) as T;
}

export const pessoasApi = {
  listar: () => request<Pessoa[]>("/pessoa"),
  criar: (dados: CriarPessoaInput) =>
    request<Pessoa>("/pessoa", { method: "POST", body: JSON.stringify(dados) }),
  deletar: (id: number) => request<void>(`/pessoa/${id}`, { method: "DELETE" }),
};

export const transacoesApi = {
  listar: () => request<Transacao[]>("/transacao"),
  criar: (dados: CriarTransacaoInput) =>
    request<Transacao>("/transacao", { method: "POST", body: JSON.stringify(dados) }),
};

export const totaisApi = {
  consultar: () => request<TotaisResponse>("/totais"),
};