# CreditFlow

O **CreditFlow** é um motor de decisão e gerenciamento de propostas de crédito desenvolvido para garantir alta disponibilidade, consistência de regras financeiras e processamento em tempo real. 

## O Problema: Gargalos na Esteira de Crédito
Instituições financeiras e fintechs frequentemente perdem conversão e assumem riscos operacionais devido a esteiras de crédito lentas, acopladas e suscetíveis a dados inconsistentes. A falta de validação na entrada (sujando a base de dados) e a lentidão na listagem de propostas geram gargalos severos no back-office, atrasando a análise de crédito e frustrando o cliente final.

## A Solução CreditFlow
Desenvolvemos este microsserviço para ser o coração de uma esteira de crédito escalável. O foco do sistema é processar, validar e transicionar os status das propostas financeiras com latência mínima e segurança absoluta.

O que entregamos em valor de negócio:
- **Blindagem de Entrada (Fail-Fast):** Nenhuma proposta avança na esteira com dados inválidos. Regras estritas (validação de CPF, limite de parcelas e valores) protegem a integridade do banco de dados e eliminam o retrabalho de analistas.
- **Eficiência de Back-Office:** Otimizamos o consumo de memória na listagem geral de propostas. Painéis administrativos podem carregar milhares de registros com latências na casa dos 12ms, garantindo fluidez para a equipe de operações.
- **Previsibilidade Financeira:** As regras de transição de status (Aprovação, Rejeição, Análise) são isoladas e protegidas por testes automatizados, garantindo *compliance* e eliminando o risco de regressões em regras críticas de negócio.

## Arquitetura Estratégica (Clean Architecture & DDD)
Para suportar o crescimento e futuras integrações, a solução foi dividida em camadas com isolamento absoluto do domínio:

* **CreditFlow.Domain:** O núcleo da aplicação. Contém as regras de negócio puras e entidades (ex: `PropostaCredito`). Zero acoplamento com frameworks externos.
* **CreditFlow.Application:** Orquestração de fluxos (Casos de Uso) e contratos de entrada/saída (DTOs).
* **CreditFlow.Infrastructure:** Acesso a dados otimizado. Na leitura em massa, desativamos o rastreamento de estado (`AsNoTracking`) do Entity Framework para maximizar a performance e reduzir consumo de CPU.
* **CreditFlow.API:** Porta de entrada limpa, focada apenas em roteamento e injeção de dependência.
* **CreditFlow.Domain.Tests:** Suíte de testes unitários (xUnit) que blinda as regras de crédito.

## Tecnologias
* **.NET 10** e **C# 13**
* **ASP.NET Core Web API**
* **PostgreSQL** (Persistência)
* **Entity Framework Core** (ORM)
* **FluentValidation** (Validação Fail-Fast)
* **xUnit** (Engenharia de Qualidade)

## Como Executar o Projeto

**Pré-requisitos:**
* SDK do .NET 10 instalado.
* Instância do PostgreSQL ativa.

**Passo a Passo:**
1. Clone o repositório:
   `git clone https://github.com/7Genesis/CreditFlow.git`
2. Acesse o diretório:
   `cd CreditFlow`
3. Configure a *string de conexão* com o banco de dados no arquivo `appsettings.json`.
4. Restaure as dependências e compile:
   `dotnet restore`
   `dotnet build`
5. Execute a API:
   `dotnet run --project CreditFlow.API`
   *(A API estará disponível no endereço configurado, ex: `http://localhost:5059`)*

## Engenharia de Qualidade
Para validar a resiliência das regras de domínio e a eficácia das validações de estado, execute a suíte de testes:
`dotnet test`
