# CreditFlow

API de propostas de crédito em **.NET 10**, organizada em Clean Architecture. É um projeto de estudo: o foco está em manter a regra de negócio no domínio, validar a entrada e cobrir a regra de status com testes.

## O que faz

- Cria uma proposta de crédito (CPF, valor solicitado e quantidade de parcelas). Toda proposta nasce **em análise**.
- Lista as propostas, da mais nova para a mais antiga, e consulta uma proposta pelo id.
- Aprova ou recusa uma proposta. A regra fica na entidade: só uma proposta em análise pode mudar de status.

## Endpoints

| Método | Rota | Corpo | Resposta |
|---|---|---|---|
| `POST` | `/api/propostas` | `{ "cpfCliente": "12345678901", "valorSolicitado": 15000, "quantidadeParcelas": 24 }` | `201` com o id da proposta; `400` se a validação falhar |
| `GET` | `/api/propostas` | | `200` com a lista |
| `GET` | `/api/propostas/{id}` | | `200`, ou `404` se não existir |
| `PUT` | `/api/propostas/{id}/status` | `{ "aprovado": true }` | `204`; `400` se a transição não for permitida |

Validação na entrada (FluentValidation): CPF com 11 dígitos, valor maior que zero e de 1 a 60 parcelas.

Exemplos prontos para rodar estão em [`CreditFlow.API/CreditFlow.API.http`](CreditFlow.API/CreditFlow.API.http).

## Arquitetura

```
CreditFlow.Domain          entidade PropostaCredito, StatusProposta e a regra de transição de status
CreditFlow.Application     casos de uso, DTOs e validadores (FluentValidation)
CreditFlow.Infrastructure  Entity Framework Core, PostgreSQL e repositório
CreditFlow.API             controllers e injeção de dependência
CreditFlow.Domain.Tests    testes de unidade (xUnit) da regra de status
```

Duas decisões que valem citar:

- O status só muda por `Aprovar()` e `Recusar()`, na própria entidade (`Status` tem `private set`).
- A listagem usa `AsNoTracking()`, porque os registros só são lidos.

## Como rodar

Pré-requisitos: SDK do .NET 10 e um PostgreSQL acessível.

1. Configure a string de conexão em `CreditFlow.API/appsettings.Development.json` (ou com `dotnet user-secrets`):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=creditflow;Username=postgres;Password=sua_senha"
     }
   }
   ```

2. Aplique as migrations (precisa da ferramenta `dotnet-ef`):

   ```bash
   dotnet ef database update --project CreditFlow.Infrastructure --startup-project CreditFlow.API
   ```

3. Suba a API:

   ```bash
   dotnet run --project CreditFlow.API
   ```

   Ela fica em `http://localhost:5059`.

Testes:

```bash
dotnet test
```

## O que ainda falta

- Testes de integração dos endpoints (hoje só o domínio tem testes).
- Validar o CPF pelos dígitos verificadores (hoje só o tamanho).
- Autenticação e paginação na listagem.
