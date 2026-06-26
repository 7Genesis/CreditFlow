# CreditFlow 

O **CreditFlow** é um microsserviço de motor e gerenciamento de propostas de crédito desenvolvido em **.NET 10** e **C#**. O projeto adota os princípios da **Clean Architecture** (Arquitetura Limpa) e do **Domain-Driven Design (DDD)** para garantir o isolamento absoluto da lógica de negócio, alta testabilidade, manutenibilidade e performance sob carga.

---

##  Tecnologias & Ecossistema

* **Runtime:** .NET 10
* **Linguagem:** C# 13
* **Framework Web:** ASP.NET Core Web API
* **Persistência & ORM:** Entity Framework Core
* **Banco de Dados:** PostgreSQL
* **Validação:** FluentValidation
* **Testes Unitários:** xUnit

---

##  Arquitetura do Sistema

A solução está dividida em camadas estritamente desacopladas, onde a dependência aponta sempre para o centro (o Domínio):

1.  **CreditFlow.Domain:** O coração da aplicação. Contém as entidades de negócio (`PropostaCredito`), enums (`StatusProposta`) e as interfaces dos repositórios (`IPropostaCreditoRepository`). Zero dependências externas ou de frameworks.
2.  **CreditFlow.Application:** Contém as regras de aplicação e orquestração de fluxos através de **Casos de Uso** (`UseCases`), como `CriarPropostaUseCase` e `ListarPropostasUseCase`, além das definições de DTOs de entrada e saída.
3.  **CreditFlow.Infrastructure:** Implementação do acesso a dados, configurações do DbContext do Entity Framework Core e repositórios concretos que realizam a comunicação com o PostgreSQL.
4.  **CreditFlow.API:** A porta de entrada da aplicação. Controladores HTTP limpos que delegam a execução diretamente para a camada de Aplicação através de Injeção de Dependência nativa.
5.  **CreditFlow.Domain.Tests:** Camada de testes unitários que blinda as regras de negócio contra regressões de código.

---

##  Funcionalidades Principais (CRUD)

* **Criar Proposta (`POST /api/propostas`):** Registra uma nova solicitação de crédito no banco de dados.
* **Obter Proposta (`GET /api/propostas/{id}`):** Recupera os detalhes estruturados de uma proposta específica via UUID.
* **Atualizar Status (`PUT /api/propostas/{id}/status`):** Altera o estado da proposta (Aprovada, Rejeitada, etc.) com base nas regras rígidas do domínio.
* **Listagem Geral Otimizada (`GET /api/propostas`):** Retorna uma matriz com todas as propostas, ordenada de forma decrescente pela data de criação.

---

##  Engenharia de Qualidade & Performance

### 1. Fail-Fast & Blindagem de Entrada (FluentValidation)
A validação de entrada é tratada de forma desacoplada antes de poluir a entidade de domínio ou o banco de dados. O `CriarPropostaInputValidator` assegura que:
* O CPF do cliente seja obrigatório e possua rigorosamente 11 dígitos.
* O valor solicitado seja estritamente maior que zero.
* A quantidade de parcelas esteja contida no intervalo seguro de 1 a 60.

### 2. Performance de Leitura (`AsNoTracking`)
Na listagem geral de propostas, a infraestrutura desativa o rastreamento de estado em memória do Entity Framework Core utilizando o método `.AsNoTracking()`. Como os dados são destinados exclusivamente à exibição (dashboard/painel administrativo), o consumo de CPU e memória do servidor é drasticamente reduzido, garantindo latências de resposta na casa dos **12ms**.

### 3. Cobertura de Testes Unitários
As transições de estado críticos da proposta de crédito e suas validações internas são protegidas por suites de testes em **xUnit**, garantindo previsibilidade e estabilidade durante o ciclo de evolução do software.

---

##  Como Executar o Projeto

### Pré-requisitos
* SDK do .NET 10 instalado.
* Instância do PostgreSQL ativa e string de conexão configurada em `appsettings.json`.

### Passo a Passo

1.  **Clonar o repositório:**
    ```bash
    git clone [https://github.com/7Genesis/CreditFlow.git](https://github.com/7Genesis/CreditFlow.git)
    cd CreditFlow
    ```

2.  **Restaurar as dependências:**
    ```bash
    dotnet restore
    ```

3.  **Compilar a solução:**
    ```bash
    dotnet build
    ```

4.  **Executar a API:**
    ```bash
    dotnet run --project CreditFlow.API
    ```
    A API estará disponível por padrão no endereço configurado (ex: `http://localhost:5059`).

5.  **Executar a Suite de Testes:**
    ```bash
    dotnet test
    ```
