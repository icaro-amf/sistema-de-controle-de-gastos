# Sistema de Controle de Gastos — Back-end
 
API REST para controle de gastos residenciais, com cadastro de pessoas, cadastro
de transações (receitas/despesas) e consulta de totais.
 
- **Linguagem/Framework:** C# / .NET 10 (ASP.NET Core Web API)
- **ORM:** Entity Framework Core
- **Banco de dados:** SQLite (arquivo local `gastos.db`) — os dados persistem
  normalmente entre execuções, sem necessidade de instalar um servidor de
  banco de dados separado.
---
 
## Estrutura do projeto
 
```
Sistema-de-Controles-de-Gastos/
├── Models/                 # Entidades: PessoaModel, TransacaoModel
├── Enums/                  # TipoTransacao (Receita/Despesa)
├── Data/                   # SistemaControleGastosDbContext (EF Core)
├── Repositories/
│   ├── Interfaces/         # IPessoaRepository, ITransacaoRepository
│   ├── PessoaRepository.cs
│   └── TransacaoRepository.cs
├── Controllers/
│   ├── PessoaController.cs
│   ├── TransacaoController.cs
│   └── TotaisController.cs
└── Program.cs               # Configuração da aplicação (DB, injeção de dependência)
```

A aplicação segue uma arquitetura em camadas simples:
 
**Controller** (recebe a requisição HTTP e aplica as regras de negócio) →
**Repository** (acessa o banco de dados via EF Core) → **DbContext** (mapeamento
das entidades para o SQLite).
 
---
 
## Como executar
 
### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
### Rodando a API
 
```bash
cd Sistema-de-Controles-de-Gastos
dotnet restore
dotnet run
```

A URL e a porta em que a API sobe (ex: `http://localhost:5220`) aparecem no
terminal ao rodar `dotnet run`, e também podem ser conferidas em
`Properties/launchSettings.json`.
 
Na primeira execução, o arquivo `gastos.db` (SQLite) é criado automaticamente
na pasta do projeto — é nele que os dados ficam persistidos entre execuções
(`Program.cs` chama `Database.EnsureCreated()` na inicialização).

### Testando os endpoints
 
Como o projeto não usa Swagger, os endpoints podem ser testados com qualquer
cliente HTTP (Postman, Insomnia, arquivo `.http`). Para requisições `POST`,
lembre-se de:
- Selecionar o corpo como **raw / JSON**;
- Garantir que o header `Content-Type: application/json` seja enviado.
---
 
## Regras de negócio implementadas
 
| Regra | Onde está implementada |
|---|---|
| Id de Pessoa e Transação gerado automaticamente | Convenção do EF Core (chave primária `int` auto-incremento no SQLite) |
| Ao deletar uma pessoa, suas transações são apagadas junto | `SistemaControleGastosDbContext.OnModelCreating` — relacionamento configurado com `OnDelete(DeleteBehavior.Cascade)` para ter a certeza da exclusão |
| Transação só pode referenciar uma pessoa existente | `TransacaoController.AdicionarNovaTransacao` busca a pessoa antes de salvar e retorna `400 Bad Request` se ela não existir |
| Menor de 18 anos só pode cadastrar despesas | `TransacaoController.AdicionarNovaTransacao`: se `pessoa.Idade < 18` e `Tipo == TipoTransacao.Receita`, retorna `400 Bad Request` com mensagem explicativa |
| Consulta de totais por pessoa (receitas, despesas, saldo) e total geral | `TotaisController.ObterTotais` |
 
---
 
## Endpoints da API
 
### Pessoas
 
| Método | Rota | Descrição | Corpo (JSON) |
|---|---|---|---|
| `GET` | `/api/pessoa` | Lista todas as pessoas cadastradas (com suas transações) | — |
| `GET` | `/api/pessoa/{id}` | Busca uma pessoa específica pelo Id | — |
| `POST` | `/api/pessoa` | Cadastra uma nova pessoa | `{ "nome": "Icaro A.", "idade": 23 }` |
| `DELETE` | `/api/pessoa/{id}` | Remove uma pessoa (e suas transações, em cascata) | — |
 
### Transações
 
| Método | Rota | Descrição | Corpo (JSON) |
|---|---|---|---|
| `GET` | `/api/transacao` | Lista todas as transações cadastradas | — |
| `POST` | `/api/transacao` | Cadastra uma nova transação | `{ "descricao": "Salário", "valor": 3000, "tipo": "0", "pessoaId": 1 }` |
 
> O campo `tipo` aceita os valores dos enums `"0"` ou `"1"`
> (correspondentes ao enum `TipoTransacao` -> `"Receita"` ou `"Despesa"` respectivamente).
 
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
      "nome": "Icaro A",
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
| `200 OK` | Operação realizada com sucesso (listagem, criação, remoção) |
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
- Não há edição/exclusão de transações, apenas criação e listagem.
