# AlimentaBem — API

> API RESTful do sistema **AlimentaBem**, uma plataforma de gestão de doações de alimentos que conecta cidadãos, organizações e administradores.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Status de Segurança Temporário](#status-de-segurança-temporário)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Executando a Aplicação](#executando-a-aplicação)
- [Documentação da API (Swagger)](#documentação-da-api-swagger)
- [Módulos e Endpoints](#módulos-e-endpoints)
- [Autenticação](#autenticação)
- [Migrações do Banco de Dados](#migrações-do-banco-de-dados)
- [Convenções de Código](#convenções-de-código)

---

## Visão Geral

O **AlimentaBem** é um sistema que permite:

- **Cidadãos** criarem cadastros e registrarem doações de alimentos.
- **Organizações** cadastrarem suas necessidades de itens.
- **Administradores** gerenciarem usuários, cargos (roles), pessoas físicas, organizações e todo o fluxo de doações.

A API é construída sobre **.NET 8** com **FastEndpoints**, seguindo os princípios de **Clean Architecture**, **DDD** e **CQRS** (separação de leitura e escrita via casos de uso).

---

## Status de Segurança Temporário

> **Atenção:** algumas rotas administrativas estão **temporariamente públicas** para destravar funcionalidades durante o desenvolvimento.
>
> Este estado é provisório e **não é recomendado para produção**.

Rotas com acesso provisório no momento:

- `GET /users`
- `PUT /user/role`
- `GET /natural-persons/admin`
- `PUT /natural-person/admin`
- `DELETE /natural-person/admin/{userId}`

---

## Tecnologias

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 8.0 | Plataforma principal |
| FastEndpoints | 5.26 | Endpoints HTTP enxutos e tipados |
| Entity Framework Core | 8.0 | ORM e migrações |
| SQL Server | — | Banco de dados relacional |
| FluentValidation | 11.9 | Validação de requisições |
| NSwag / Swagger | 14.0 / 6.4 | Documentação da API |
| JWT Bearer (RSA-256) | — | Autenticação stateless |
| BCrypt.Net-Next | 4.0 | Hash de senhas |
| dotenv.net | 3.1 | Leitura de variáveis de ambiente |

---

## Estrutura do Projeto

```
alimenta-bem-api/
├── AlimentaBem.csproj          # Arquivo de projeto .NET
├── Program.cs                  # Bootstrap: DI, middlewares, configurações
├── appsettings.json            # Configurações base
├── .env                        # Variáveis de ambiente (não versionar)
├── public.key / private.key    # Par de chaves RSA para JWT
│
├── Context/
│   └── DbContext.cs            # EF Core DbContext com todas as entidades
│
├── DataMappings/               # Configurações Fluent API por entidade
│   ├── Donation/
│   ├── NaturalPerson/
│   ├── Organization/
│   ├── OrganizationRequirement/
│   ├── Role/
│   └── User/
│
├── EntityMetadata/             # Classes base e interfaces reutilizáveis
│   ├── BaseEntity.cs           # Id (Guid) gerado automaticamente
│   ├── WithTimeStamp.cs        # CreatedAt / UpdatedAt
│   └── Interface/
│       ├── IAuditable.cs
│       └── ISoftDelete.cs      # Exclusão lógica (DeletedAt)
│
├── Helpers/                    # Utilitários transversais
│   ├── DependencyInjectionConfig.cs
│   ├── FormatPassword.cs
│   ├── EmailValidation.cs
│   └── I18N/                   # Internacionalização (pt-BR / en-US)
│
├── Languages/                  # Arquivos de tradução JSON por módulo
│
├── Migrations/                 # Migrações EF Core geradas
│
└── Src/
    ├── Modules/                # Módulos de domínio (DDD)
    │   ├── User/
    │   │   ├── UseCases/
    │   │   │   ├── Authenticate/
    │   │   │   ├── Create/
    │   │   │   └── ReadOne/
    │   │   └── Repository/
    │   ├── NaturalPerson/
    │   │   ├── UseCases/
    │   │   │   ├── ReadList/
    │   │   │   ├── ReadOne/
    │   │   │   └── Update/
    │   │   └── Repository/
    │   ├── Organization/
    │   │   ├── UseCases/ (Create, ReadList)
    │   │   └── Repository/
    │   ├── OrganizationRequirement/
    │   │   ├── UseCases/
    │   │   └── Repository/
    │   ├── Donation/
    │   │   ├── UseCases/
    │   │   └── Repository/
    │   └── Role/
    │       ├── UseCases/ Repository/ Enum/
    │
    └── Providers/
        └── Crypto/             # Serviço de criptografia (RSA + BCrypt)
```

Cada caso de uso contém seus próprios arquivos: `Endpoint.cs`, `Request.cs`, `Response.cs` e `Validator.cs`.

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou SQL Server Express)
- [EF Core CLI Tools](https://learn.microsoft.com/pt-br/ef/core/cli/dotnet)
- [OpenSSL](https://slproweb.com/products/Win32OpenSSL.html) (para gerar as chaves RSA)

```bash
dotnet tool install --global dotnet-ef
```

---

## Configuração do Ambiente

### 1. String de conexão

Edite `appsettings.json` com os dados do seu SQL Server:

```json
"ConnectionStrings": {
  "sqlConnection": "Server=SEU_SERVIDOR;Database=NOME_DO_BANCO;Integrated Security=True;TrustServerCertificate=True;"
}
```

### 2. Variáveis de ambiente

Crie um arquivo `.env` na raiz do projeto:

```env
JWT_SECRET=sua_chave_secreta_aqui
API_KEY=sua_api_key_aqui
```

### 3. Chaves RSA para JWT

Os arquivos `public.key` e `private.key` devem estar na raiz do projeto. Para gerar um novo par:

```bash
openssl genrsa -out private.key 2048
openssl rsa -in private.key -pubout -out public.key
```

---

## Executando a Aplicação

```bash
# Na pasta alimenta-bem-api/
dotnet restore
dotnet run
```

A API estará disponível em `http://localhost:5178` (HTTP) por padrão — a porta pode ser configurada em `Properties/launchSettings.json`.

---

## Documentação da API (Swagger)

Com a aplicação em execução no ambiente de desenvolvimento, acesse:

```
http://localhost:5178/swagger
```

---

## Módulos e Endpoints

### User

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/user` | Cria novo usuário | Público |
| `POST` | `/user/authenticate` | Autentica e retorna JWT | Público |
| `GET` | `/user/{userId}` | Busca usuário por ID | Público |
| `GET` | `/users` | Lista usuários com role atual | Público (temporário) |
| `PUT` | `/user/role` | Atualiza role de usuário existente | Público (temporário) |

### NaturalPerson
Perfil da pessoa física vinculada a um usuário.

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `GET` | `/natural-persons` | Lista simplificada de pessoas físicas | Público |
| `GET` | `/natural-person/{userId}` | Busca perfil por ID de usuário | Público |
| `PUT` | `/natural-person` | Cria/atualiza perfil de pessoa física | Público |
| `POST` | `/natural-person/admin` | Cria ou atualiza doador (com credenciais de usuário) | Admin |
| `GET` | `/natural-persons/admin` | Lista completa de doadores com total de doações | Público (temporário) |
| `PUT` | `/natural-person/admin` | Atualiza dados de doador por admin | Público (temporário) |
| `DELETE` | `/natural-person/admin/{userId}` | Exclui doador por userId (soft delete) | Público (temporário) |

### Organization
Organizações que recebem doações.

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/organization` | Cadastra organização | Admin |
| `GET` | `/organizations` | Lista organizações | Autenticado |

### OrganizationRequirement
Necessidades declaradas pelas organizações (itens que precisam receber).

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/organization-requirement` | Cria necessidade | Admin |
| `GET` | `/organization-requirements/{organizationId}` | Lista necessidades de uma organização | Público |
| `PUT` | `/organization-requirement` | Atualiza necessidade existente | Admin |
| `DELETE` | `/organization-requirement/{id}` | Remove necessidade (soft delete) | Admin |

### Donation
Doações de alimentos feitas por cidadãos.

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/donation` | Registra doação | Citizen |

### Role
Papéis de acesso do sistema. Gerenciado internamente — valores possíveis: `Admin`, `Developer`, `Citizen`.

---

## Autenticação

A API usa **JWT com RSA-256** (assinatura assimétrica):

1. Faça `POST /user/authenticate` com o body:
   ```json
   {
     "email": "usuario@email.com",
     "password": "senha"
   }
   ```
2. Receba o token no campo `accessToken` da resposta.
3. Inclua o token em todas as requisições protegidas:
   ```
   Authorization: Bearer <accessToken>
   ```

**Roles disponíveis:**
- `Admin` — acesso total ao sistema
- `Developer` — perfil técnico com permissões específicas de ambiente
- `Citizen` — acesso às rotas de doação e perfil próprio

---

## Migrações do Banco de Dados

```bash
# Aplicar todas as migrações pendentes
dotnet ef database update

# Criar nova migração após alterar entidades
dotnet ef migrations add NomeDaMigracao

# Reverter para uma migração específica
dotnet ef database update NomeDaMigracaoAnterior
```

---

## Convenções de Código

- **Namespace raiz:** `AlimentaBem`
- **Padrão de namespace:** `AlimentaBem.Src.Modules.{Modulo}.{Camada}`
- **Nomenclatura:** PascalCase para classes, métodos e propriedades; inglês em todo o código
- **Casos de uso:** cada funcionalidade tem seu próprio diretório com `Endpoint`, `Request`, `Response` e `Validator`
- **Soft Delete:** implementado via `ISoftDelete` — registros nunca são apagados fisicamente do banco
- **I18N:** respostas de erro localizadas via header `Accept-Language` (`pt-BR` e `en-US`)
- **CORS:** em desenvolvimento, permite requisições de `http://localhost:3001`
