# AlimentaBem — UI

> Interface web do sistema **AlimentaBem**, uma plataforma de gestão de doações de alimentos que conecta cidadãos, organizações e administradores.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Status de Segurança Temporário](#status-de-segurança-temporário)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Executando a Aplicação](#executando-a-aplicação)
- [Rotas da Aplicação](#rotas-da-aplicação)
- [Autenticação e Controle de Acesso](#autenticação-e-controle-de-acesso)
- [Convenções de Código](#convenções-de-código)

---

## Visão Geral

O frontend do **AlimentaBem** é uma Single Page Application (SPA) em **React** que permite:

- **Cidadãos** se registrarem, fazerem login e registrarem doações de alimentos.
- **Administradores** gerenciarem pessoas físicas, organizações, necessidades, usuários/cargos e visualizarem dados do sistema.

A comunicação com o backend é feita via **axios** e toda a autenticação é baseada em **JWT** armazenado no `localStorage`.

---

## Status de Segurança Temporário

> **Atenção:** parte das rotas administrativas da API está temporariamente configurada como pública.
>
> A UI já possui guardas de rota por perfil, mas esse cenário no backend é provisório e deve ser corrigido antes de produção.

Rotas administrativas com acesso público temporário atualmente documentadas na API:

- `GET /users`
- `PUT /user/role`
- `GET /natural-persons/admin`
- `PUT /natural-person/admin`
- `DELETE /natural-person/admin/{userId}`

---

## Tecnologias

| Tecnologia | Versão | Finalidade |
|---|---|---|
| React | 18.3 | Biblioteca de UI |
| React Router DOM | 7.6 | Roteamento SPA |
| Vite | 6.4 | Build tool e servidor de desenvolvimento |
| Ant Design | 5.x | Biblioteca de componentes UI (forms, modais, tabelas, layouts) |
| axios | 1.7 | Requisições HTTP à API |
| jwt-decode | 4.0 | Decodificação do token JWT no cliente |
| web-vitals | 2.1 | Métricas de performance |

---

## Estrutura do Projeto

```
alimenta-bem-ui/
├── index.html              # Entrada HTML do Vite (raiz do projeto)
├── vite.config.js          # Configuração do Vite
├── package.json
├── .env                    # Variáveis de ambiente (não versionar)
├── public/                 # Arquivos estáticos públicos
│   ├── favicon.ico
│   ├── manifest.json
│   └── robots.txt
│
└── src/
    ├── index.jsx           # Ponto de entrada da aplicação React
    ├── App.jsx             # Roteamento principal e guards de role
    ├── App.css
    ├── index.css
    │
    ├── Components/         # Páginas e componentes de funcionalidade
    │   ├── Login.jsx                          # Tela de login
    │   ├── CreateUser.jsx                     # Cadastro de novo usuário
    │   ├── CreateNaturalPerson.jsx            # Cadastro de pessoa física
    │   ├── CreateOrganization.jsx             # Cadastro de organização
    │   ├── CreateOrganizationRequirement.jsx  # Cadastro de necessidade
    │   ├── CreateDonation.jsx                 # Formulário de doação
    │   ├── UserHome.jsx                       # Página inicial do cidadão
    │   ├── AdminHome.jsx                      # Painel de gestão do administrador
    │   ├── Profile.jsx                        # Perfil do usuário
    │   └── LoggedUser.jsx
    │
    ├── Layouts/            # Layouts compartilhados por grupo de rotas
    │   ├── PublicLayout.jsx              # Rotas públicas (login, cadastro)
    │   ├── DashboardLayout.jsx           # Layout Ant Design: sidebar responsiva com Drawer mobile
    │   ├── LoggedUserLayout.jsx          # Rotas do cidadão autenticado (usa DashboardLayout)
    │   ├── AdminUserLayout.jsx           # Rotas do administrador (usa DashboardLayout)
    │   └── ProtectedLayoutRouter.jsx     # Guard de rota com verificação de role
    │
    └── Css/                # Estilos globais
        └── Style.css       # Variáveis CSS, utilititários e classes de layout
```

---

## Pré-requisitos

- [Node.js](https://nodejs.org/) 18 ou superior
- npm 9 ou superior
- A [API AlimentaBem](../alimenta-bem-api/README.md) em execução

---

## Configuração do Ambiente

Crie um arquivo `.env` na raiz do projeto (`alimenta-bem-ui/`):

```env
VITE_API_BASE_URL=http://localhost:5178
```

Use essa variável nas chamadas axios:
```js
axios.get(`${import.meta.env.VITE_API_BASE_URL}/natural-persons`)
```

> **Atenção:** variáveis de ambiente no Vite devem obrigatoriamente começar com `VITE_` para serem expostas ao navegador. Variáveis com prefixo `REACT_APP_` (Create React App) **não funcionam** com Vite.

---

## Executando a Aplicação

```bash
# Instalar dependências
npm install

# Iniciar servidor de desenvolvimento (http://localhost:3001)
npm start

# Gerar build de produção (saída em /dist)
npm run build

# Pré-visualizar o build de produção localmente
npm run preview
```

---

## Rotas da Aplicação

### Rotas Públicas (`/`)
Acessíveis sem autenticação.

| Rota | Componente | Descrição |
|---|---|---|
| `/login` | `Login` | Tela de login |
| `/create-user` | `CreateUser` | Cadastro de novo usuário |

### Rotas do Cidadão (`/logged-user`)
Requerem autenticação com role `Citizen`.

| Rota | Componente | Descrição |
|---|---|---|
| `/logged-user` | `UserHome` | Página inicial do cidadão |
| `/logged-user/create-donation` | `CreateDonation` | Registrar nova doação (doador auto-preenchido via JWT; necessidades da instituição exibidas como sugestões) |
| `/logged-user/profile` | `Profile` | Perfil do usuário (campo PCD como toggle Switch) |

### Rotas do Administrador (`/admin`)
Requerem autenticação com role `Admin`.

| Rota | Componente | Descrição |
|---|---|---|
| `/admin` | `AdminHome` | Painel de gestão: estatísticas, necessidades por instituição, editar/excluir inline, filtros e paginação |
| `/admin/profile` | `Profile` | Perfil do administrador |
| `/admin/create-person` | `CreateNaturalPerson` | Cadastrar/atualizar doador com credenciais |
| `/admin/create-organization` | `CreateOrganization` | Cadastrar organização |
| `/admin/organization-req` | `CreateOrganizationRequirement` | Cadastrar necessidade de organização |

### Funcionalidades no Painel Admin

- Modal de **Gerenciar usuários e cargos** (listar usuários e alterar role).
- Modal de **Gerenciar doadores** (listar, editar, excluir e visualizar total de doações por doador).

---

## Autenticação e Controle de Acesso

O fluxo de autenticação funciona da seguinte forma:

1. O usuário faz login em `/login`.
2. O token JWT retornado pela API é salvo em `localStorage` com a chave `accessToken`.
3. O componente `RequireAccess` (em `App.jsx`) decodifica o token, valida expiração e verifica roles permitidas e negadas por área.
4. Se não autenticado, token inválido/expirado ou role incompatível com a rota, o usuário é redirecionado para a área correta ou para `/login`.
5. A navegação por URL digitada/colada também é validada, evitando acesso cruzado entre áreas (ex.: admin acessando rotas de cidadão).

```
Token → localStorage (accessToken)
         ↓
    RequireAccess valida token + regras de rota
         ↓
    ✅ Acesso liberado  ou  🔒 Redireciona para /login
```

---

## Convenções de Código

- **Extensão:** todos os arquivos com JSX usam `.jsx`
- **Nomenclatura:** PascalCase para componentes e arquivos; inglês em todo o código
- **Pastas:** PascalCase (`Components/`, `Layouts/`, `Css/`)
- **Importações:** sempre com extensão explícita (`.jsx`) para compatibilidade com Vite
- **Variáveis de ambiente:** prefixo `VITE_` obrigatório para exposição no cliente
- **Estilização:** Ant Design para componentes interativos + `Style.css` com CSS custom properties para customizações globais
