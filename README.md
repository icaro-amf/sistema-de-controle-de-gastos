# Sistema de Controle de Gastos Residenciais
 
Aplicação full-stack para controle de gastos residenciais, com cadastro de
pessoas, cadastro de transações (receitas/despesas) e consulta de totais.
 
- **Back-end:** C# / .NET 10 (ASP.NET Core Web API) + Entity Framework Core
- **Front-end:** React + TypeScript (Vite)
- **Banco de dados:** SQLite (arquivo local `gastos.db`) — os dados persistem
  normalmente entre execuções, sem necessidade de instalar um servidor de
  banco separado.
---
 
## Estrutura do projeto
 
```
Sistema-de-Controles-de-Gastos/
├── Controllers/
│   ├── PessoaController.cs
│   ├── TransacaoController.cs
│   └── TotaisController.cs
├── Models/                  # Entidades: PessoaModel, TransacaoModel
├── Enums/                   # TipoTransacao (Receita/Despesa)
├── Data/                    # SistemaControleGastosDbContext (EF Core)
├── Repositories/
│   ├── Interfaces/          # IPessoaRepository, ITransacaoRepository
│   ├── PessoaRepository.cs
│   └── TransacaoRepository.cs
├── Program.cs                # Configuração da aplicação (DB, CORS, injeção de dependência)
└── frontend/                  # Aplicação React + TypeScript
    └── src/
        ├── types.ts            # Tipos TypeScript espelhando os Models do back-end
        ├── api.ts               # Camada de acesso HTTP à API
        ├── App.tsx               # Componente raiz
        └── components/            # PessoaForm, TransacaoForm, TransacoesList, Totais
```
 
A aplicação segue uma arquitetura em camadas simples no back-end:
 
**Controller** (recebe a requisição HTTP e aplica as regras de negócio) →
**Repository** (acessa o banco de dados via EF Core) → **DbContext**
(mapeamento das entidades para o SQLite).
 
---
 
## Como executar
 
### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
### 1. Back-end (API)
 
```bash
cd Sistema-de-Controles-de-Gastos
dotnet restore
dotnet run
```
 
A API sobe fixamente em **`http://localhost:5000`** (porta configurada em
`Properties/launchSettings.json`).
 
Na primeira execução, o arquivo `gastos.db` (SQLite) é criado automaticamente
na pasta do projeto — é nele que os dados ficam persistidos entre execuções
(`Program.cs` chama `Database.EnsureCreated()` na inicialização).
 
### 2. Front-end
 
Em outro terminal:
 
```bash
cd Sistema-de-Controles-de-Gastos/frontend
npm install
npm run dev
```
 
A aplicação abre em **`http://localhost:5173`** e já está configurada para
conversar com o back-end em `http://localhost:5000` (ver `src/api.ts`).
 
> **Importante:** o back-end precisa estar rodando **antes** de abrir o
> front-end, já que a tela carrega os dados da API assim que é aberta.
 
### Testando os endpoints diretamente (opcional)
 
Como o projeto não usa Swagger, os endpoints também podem ser testados com
qualquer cliente HTTP (Postman, Insomnia, ou o arquivo
`Sistema-de-Controles-de-Gastos.http` incluso no projeto). Para requisições
`POST`, selecione o corpo como **raw / JSON** e garanta que o header
`Content-Type: application/json` seja enviado.
 
---
 
## Regras de negócio implementadas
 
| Regra | Onde está implementada |
|---|---|
| Id de Pessoa e Transação gerado automaticamente | Convenção do EF Core (chave primária `int` auto-incremento no SQLite) |
| Ao deletar uma pessoa, suas transações são apagadas junto | `SistemaControleGastosDbContext.OnModelCreating` — relacionamento configurado com `OnDelete(DeleteBehavior.Cascade)` |
| Transação só pode referenciar uma pessoa existente | `TransacaoController.AdicionarNovaTransacao` busca a pessoa antes de salvar e retorna `400 Bad Request` se ela não existir |
| Menor de 18 anos só pode cadastrar despesas | `TransacaoController.AdicionarNovaTransacao`: se `pessoa.Idade < 18` e `Tipo == TipoTransacao.Receita`, retorna `400 Bad Request` com mensagem explicativa |
| Consulta de totais por pessoa (receitas, despesas, saldo) e total geral | `TotaisController.ObterTotais` |
 
---
 
## Endpoints da API
 
### Pessoas
 
| Método | Rota | Descrição | Corpo (JSON) |
|---|---|---|---|
| `GET` | `/api/pessoa` | Lista todas as pessoas cadastradas | — |
| `GET` | `/api/pessoa/{id}` | Busca uma pessoa específica pelo Id | — |
| `POST` | `/api/pessoa` | Cadastra uma nova pessoa | `{ "nome": "Maria Silva", "idade": 30 }` |
| `DELETE` | `/api/pessoa/{id}` | Remove uma pessoa (e suas transações, em cascata) | — |
 
### Transações
 
| Método | Rota | Descrição | Corpo (JSON) |
|---|---|---|---|
| `GET` | `/api/transacao` | Lista todas as transações cadastradas | — |
| `POST` | `/api/transacao` | Cadastra uma nova transação | `{ "descricao": "Salário", "valor": 3000, "tipo": "Receita", "pessoaId": 1 }` |
 
> O campo `tipo` aceita os valores de texto `"Receita"` ou `"Despesa"`
> (o `Program.cs` configura um `JsonStringEnumConverter` para que o enum
> `TipoTransacao` seja serializado como texto, não como número).
 
### Totais
 
| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/totais` | Retorna, para cada pessoa, o total de receitas, total de despesas e saldo; ao final, o total geral somando todas as pessoas |
 
**Exemplo de resposta de `GET /api/totais`:**
 
```json
{
  "totaisPorPessoa": [
    {
      "pessoaId": 1,
      "nome": "Maria Silva",
      "totalReceitas": 3000,
      "totalDespesas": 500,
      "saldo": 2500
    }
  ],
  "totalGeralReceitas": 3000,
  "totalGeralDespesas": 500,
  "saldoGeral": 2500
}
```
 
---
 
## Códigos de status utilizados
 
| Status | Quando ocorre |
|---|---|
| `200 OK` | Operação realizada com sucesso (listagem, criação) |
| `204 No Content` | Remoção de pessoa realizada com sucesso |
| `400 Bad Request` | Dados inválidos (nome vazio, idade negativa, valor ≤ 0, pessoa inexistente, tentativa de cadastrar receita para menor de idade) |
| `404 Not Found` | Id informado não corresponde a nenhuma pessoa cadastrada |
 
---
 
## Decisões técnicas
 
- **SQLite** foi escolhido por ser um banco de arquivo único, sem exigir a
  instalação de um servidor de banco separado — atende ao requisito de
  persistência com o mínimo de fricção para rodar o projeto em qualquer
  máquina.
- **Repository Pattern** (`Interfaces/` + implementações) separa o acesso a
  dados da lógica dos controllers, facilitando manutenção e testes.
- **Cascade delete a nível de banco** (configurado no `OnModelCreating`, em
  vez de apagar as transações manualmente no código) garante consistência
  mesmo se os dados forem manipulados por outro caminho.
- **CORS** habilitado no back-end (`Program.cs`) liberando explicitamente a
  origem `http://localhost:5173`, para permitir que o front-end React
  consuma a API durante o desenvolvimento.
- Não há edição/exclusão de transações, apenas criação e listagem.
