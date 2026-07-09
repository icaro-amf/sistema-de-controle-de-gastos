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
