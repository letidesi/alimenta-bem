# AlimentaBem — API

> API RESTful do sistema **AlimentaBem**, uma plataforma de gestão de doações de alimentos que conecta cidadãos, organizações sociais e administradores.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Executando a Aplicação](#executando-a-aplicação)
- [Documentação da API (Swagger)](#documentação-da-api-swagger)
- [Módulos e Endpoints](#módulos-e-endpoints)
- [Autenticação](#autenticação)
- [Segurança](#segurança)
- [Migrações do Banco de Dados](#migrações-do-banco-de-dados)
- [Internacionalização](#internacionalização)
- [Convenções de Código](#convenções-de-código)

---

## Visão Geral

O **AlimentaBem API** é construída sobre **.NET 8** com **FastEndpoints**, seguindo princípios de **DDD** e **CQRS** (separação de leitura e escrita via casos de uso).

Suporta quatro perfis de acesso:

- **Cidadão** — registra doações e gerencia o próprio perfil.
- **Administrador** — gerencia usuários, doadores, organizações vinculadas e fila de doações.
- **Developer** — acesso global: gerencia todos os usuários, todas as instituições e vínculos.
- **Público** — consulta organizações e necessidades cadastradas.

---

## Tecnologias

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 8.0 | Plataforma principal |
| FastEndpoints | 5.26 | Endpoints HTTP enxutos e tipados |
| Entity Framework Core | 8.0 | ORM e migrações |
| PostgreSQL (Neon) | — | Banco de dados relacional |
| FluentValidation | 11.9 | Validação de requisições |
| NSwag | 14.0 | Documentação Swagger |
| JWT Bearer RS256 | — | Autenticação stateless com chaves RSA |
| BCrypt.Net-Next | 4.0 | Hash de senhas (fator 12) |
| Resend | 0.5 | Envio de e-mails transacionais |
| dotenv.net | 3.1 | Leitura de variáveis de ambiente |

---

## Estrutura do Projeto

```
alimenta-bem-api/
├── AlimentaBem.csproj
├── Program.cs                  # Bootstrap: DI, middlewares, segurança, rate limiting
├── appsettings.json
├── .env                        # Variáveis de ambiente (não versionar)
├── public.key / private.key    # Par de chaves RSA para JWT (opcional se usar env vars)
│
├── Context/
│   └── DbContext.cs            # EF Core DbContext com ApplyConfigurationsFromAssembly
│
├── DataMappings/               # Configurações Fluent API por entidade
│   ├── Donation/
│   ├── NaturalPerson/
│   ├── Organization/
│   ├── OrganizationRequirement/
│   ├── PasswordReset/
│   ├── Role/
│   └── User/
│
├── EntityMetadata/             # Classes base reutilizáveis
│   ├── BaseEntity.cs           # Id (Guid) gerado automaticamente
│   ├── WithTimeStamp.cs        # CreatedAt / UpdatedAt / DeletedAt (DateTimeOffset)
│   └── Interface/
│       ├── IAuditable.cs
│       └── ISoftDelete.cs
│
├── Helpers/                    # Utilitários transversais
│   ├── DependencyInjectionConfig.cs
│   ├── FormatPassword.cs       # BCrypt fator 12
│   ├── ValidationNaturalPerson.cs
│   ├── AdminOrganizationGuard.cs
│   └── I18N/                   # Internacionalização (pt-BR / en-US)
│
├── Languages/                  # Arquivos de tradução JSON por módulo
│
├── Migrations/                 # Migrações EF Core geradas
│
└── Src/
    ├── Modules/
    │   ├── User/               # Authenticate, Create, AdminCreate, ReadOne, ReadList, Delete, UpdateRole
    │   ├── NaturalPerson/      # ReadOne, ReadList, AdminReadList, Update, AdminUpsert, AdminDelete
    │   ├── Organization/       # Create, ReadList, Update, Delete
    │   ├── OrganizationRequirement/ # Create, ReadListByOrganization, Update, Delete
    │   ├── Donation/           # Create, ReadListByNaturalPerson, ReadListByOrganization, UpdateStatus
    │   ├── UserOrganization/   # Create (vincular), Delete (desvincular)
    │   ├── PasswordReset/      # ForgotPassword, ResetPassword
    │   └── Role/
    └── Providers/
        └── Crypto/             # CryptoService — RSA singleton cacheado
```

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [EF Core CLI](https://learn.microsoft.com/pt-br/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`
- Banco PostgreSQL (ou conta gratuita no [Neon](https://neon.tech))
- [OpenSSL](https://slproweb.com/products/Win32OpenSSL.html) para gerar as chaves RSA

---

## Configuração do Ambiente

### 1. Variáveis de ambiente

Crie um arquivo `.env` na raiz de `alimenta-bem-api/` (use `.env.example` como base):

```env
DATABASE_URL=Host=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true
RESEND_API_KEY=re_...
FRONTEND_URL=http://localhost:3000
RSA_PRIVATE_KEY=        # conteúdo do private.key com \n no lugar das quebras de linha
RSA_PUBLIC_KEY=         # conteúdo do public.key com \n no lugar das quebras de linha
```

> Se `RSA_PRIVATE_KEY` / `RSA_PUBLIC_KEY` estiverem vazios, a API lê os arquivos `private.key` e `public.key` da pasta raiz automaticamente.

### 2. Chaves RSA

```bash
openssl genrsa -out private.key 2048
openssl rsa -in private.key -pubout -out public.key
```

### 3. Banco de dados

```bash
dotnet ef database update
```

---

## Executando a Aplicação

```bash
cd alimenta-bem-api
dotnet restore
dotnet run
```

A API sobe em `http://localhost:5178` por padrão (configurável em `Properties/launchSettings.json`).

---

## Documentação da API (Swagger)

Disponível nos ambientes de desenvolvimento e staging:

```
http://localhost:5178/swagger
```

---

## Módulos e Endpoints

### User

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/user` | Cadastra novo usuário | Público |
| `POST` | `/user/authenticate` | Autentica e retorna JWT | Público (rate limit: 5/min) |
| `POST` | `/user/admin` | Cria usuário com role e vínculos | Admin, Developer |
| `GET` | `/user/{userId}` | Busca usuário por ID | Autenticado |
| `GET` | `/users` | Lista todos os usuários | Admin, Developer |
| `PUT` | `/user/role` | Atualiza role de usuário | Admin, Developer |
| `DELETE` | `/user/{userId}` | Exclui usuário (soft delete, libera e-mail) | Admin, Developer |

### NaturalPerson

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `GET` | `/natural-persons` | Lista simplificada (id, nome) | Público |
| `GET` | `/natural-person/{userId}` | Busca perfil — cidadão só acessa o próprio | Autenticado |
| `PUT` | `/natural-person` | Cria ou atualiza perfil (upsert) — userId forçado do JWT para Citizen | Autenticado |
| `GET` | `/natural-persons/admin` | Lista completa de doadores com total de doações | Admin |
| `POST` | `/natural-person/admin` | Cria ou atualiza doador com credenciais | Admin |
| `PUT` | `/natural-person/admin` | Atualiza dados de doador | Admin |
| `DELETE` | `/natural-person/admin/{userId}` | Exclui doador (soft delete) | Admin |

### Organization

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/organization` | Cadastra organização | Admin |
| `GET` | `/organizations` | Lista organizações ativas | Autenticado |
| `PUT` | `/organization` | Atualiza organização | Admin |
| `DELETE` | `/organization/{id}` | Exclui organização + vínculos de usuários (soft delete) | Admin, Developer |

### OrganizationRequirement

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/organization-requirement` | Cria necessidade | Admin |
| `GET` | `/organization-requirements/{organizationId}` | Lista necessidades de uma organização | Público |
| `PUT` | `/organization-requirement` | Atualiza necessidade | Admin |
| `DELETE` | `/organization-requirement/{id}` | Remove necessidade | Admin |

### Donation

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/donation` | Registra doação | Citizen |
| `GET` | `/donations/natural-person/{naturalPersonId}` | Histórico do doador | Citizen |
| `GET` | `/donations/organization/{organizationId}` | Fila por instituição | Admin |
| `PUT` | `/donation/status` | Atualiza status | Admin |

### UserOrganization

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/user-organization` | Vincula usuário a uma organização | Admin, Developer |
| `DELETE` | `/user-organization` | Desvincula usuário de uma organização | Admin, Developer |

### PasswordReset

| Método | Rota | Descrição | Acesso |
|---|---|---|---|
| `POST` | `/user/forgot-password` | Envia e-mail com link de reset | Público (rate limit: 3/hora) |
| `POST` | `/user/reset-password` | Redefine senha com token válido | Público (rate limit: 5/hora) |

---

## Autenticação

A API usa **JWT RS256** (assinatura assimétrica com par de chaves RSA):

1. `POST /user/authenticate` com `{ "email": "...", "password": "..." }`
2. Use o `accessToken` retornado no header de todas as requisições protegidas:
   ```
   Authorization: Bearer <accessToken>
   ```

**Roles disponíveis:**

| Role | Permissões |
|---|---|
| `Citizen` | Perfil próprio, doações próprias |
| `Admin` | Usuários e doadores das suas organizações, fila de doações, necessidades |
| `Developer` | Todos os usuários, todas as organizações, vínculos |

---

## Segurança

- **JWT RS256** — par de chaves RSA 2048 bits; chave privada nunca exposta ao cliente
- **BCrypt fator 12** — senha mínima de 12 e máxima de 128 caracteres
- **Reset de senha seguro** — token gerado com `RandomNumberGenerator`, apenas o hash SHA-256 é armazenado no banco
- **Rate limiting nativo ASP.NET 8:**
  - `/user/authenticate` — 5 tentativas por IP por minuto
  - `/user/forgot-password` — 3 pedidos por IP por hora
  - `/user/reset-password` — 5 usos por IP por hora
- **IDOR protegido** — Citizen só acessa e edita o próprio perfil (userId forçado do JWT)
- **Soft delete com índice parcial** — e-mail único apenas entre registros ativos (`WHERE deletedAt IS NULL`)
- **Headers de segurança HTTP** — X-Content-Type-Options, X-Frame-Options, Referrer-Policy, Permissions-Policy, X-XSS-Protection
- **CORS restrito** — origens, métodos e headers explicitamente configurados

---

## Migrações do Banco de Dados

```bash
# Aplicar migrações pendentes
dotnet ef database update

# Criar nova migração após alterar entidades
dotnet ef migrations add NomeDaMigracao

# Reverter para migração específica
dotnet ef database update NomeDaMigracaoAnterior
```

---

## Internacionalização

Respostas de erro são localizadas via header `Accept-Language` (`pt-BR` e `en-US`). Os arquivos de tradução ficam em `Languages/` no formato `{modulo}.resource.json`.

---

## Convenções de Código

- **Namespace raiz:** `AlimentaBem`
- **Padrão:** `AlimentaBem.Src.Modules.{Modulo}.{Camada}`
- **Nomenclatura:** PascalCase para classes e métodos; inglês em todo o código
- **Casos de uso:** cada funcionalidade tem seu próprio diretório com `Endpoint`, `Request`, `Response` e `Validator`
- **Soft Delete:** via `ISoftDelete` — registros nunca são apagados fisicamente
- **Timestamps:** `DateTimeOffset` em todas as entidades para compatibilidade com PostgreSQL
