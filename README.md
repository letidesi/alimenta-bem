# AlimentaBem

> Plataforma de gestão de doações de alimentos que conecta cidadãos, organizações sociais e administradores.

---

## O que é o AlimentaBem?

O **AlimentaBem** nasceu da necessidade de organizar e acompanhar doações de alimentos de forma simples e transparente. Muitas vezes doações se perdem por falta de comunicação entre quem quer ajudar e quem precisa receber.

A plataforma resolve isso conectando quatro perfis:

- **Cidadão** — faz login, completa o perfil, registra doações de alimentos e acompanha o status de cada uma em tempo real.
- **Organização** — instituições como igrejas, ONGs e escolas cadastram suas necessidades e gerenciam a fila de doações recebidas.
- **Administrador** — gerencia usuários, cargos, doadores e organizações vinculadas à sua instituição.
- **Developer** — acesso global: gerencia todos os usuários, todas as instituições e seus vínculos.

---

## Fluxo de uma doação

```
Cidadão registra doação
        ↓
    [Enviada]
        ↓
  [Em análise]  ←──────────────────────┐
        ↓                              │
[Pronta p/ entrega]    [Indisponível temporariamente]
        ↓                              ↑
   [Recebida]    ←── pode redirecionar para outra instituição
```

Quando uma instituição está temporariamente indisponível para receber, o cidadão recebe uma mensagem amigável orientando a escolher outra instituição ativa, garantindo que a ajuda não seja desperdiçada.

---

## Estrutura do repositório

```
alimenta-bem/
├── alimenta-bem-api/   # Backend — .NET 8 + FastEndpoints + EF Core + PostgreSQL
└── alimenta-bem-ui/    # Frontend — React 18 + Vite + Ant Design
```

---

## Tecnologias

| Camada | Stack |
|--------|-------|
| Backend | .NET 8, FastEndpoints, Entity Framework Core 8 |
| Frontend | React 18, Vite, Ant Design, Axios |
| Auth | JWT RS256 (chave RSA), BCrypt (fator 12) |
| Banco | PostgreSQL (Neon) com migrations via EF Core |
| Email | Resend API |
| Deploy | Render (API) + Vercel (UI) |

---

## Segurança

- Autenticação JWT com par de chaves RSA (RS256)
- Senhas com BCrypt fator 12, mínimo 12 caracteres
- Tokens de reset de senha com hash SHA-256 armazenado no banco
- Rate limiting: 5 tentativas de login/min, 3 pedidos de reset/hora
- CORS restrito às origens configuradas
- Headers de segurança HTTP (X-Frame-Options, CSP, HSTS etc.)
- Índice único parcial em `email` — libera o e-mail ao excluir conta (soft delete)
- IDOR protegido: cidadão só acessa e edita o próprio perfil

---

## Início rápido

### Pré-requisitos

- .NET 8 SDK
- Node.js 18+
- PostgreSQL (ou conta no [Neon](https://neon.tech))
- Par de chaves RSA (veja abaixo)

### Gerar chaves RSA

```bash
openssl genrsa -out private.key 2048
openssl rsa -in private.key -pubout -out public.key
```

Coloque `private.key` e `public.key` na raiz de `alimenta-bem-api/`, ou configure as variáveis de ambiente `RSA_PRIVATE_KEY` e `RSA_PUBLIC_KEY` com o conteúdo dos arquivos (substituindo quebras de linha por `\n`).

### API

```bash
cd alimenta-bem-api
cp .env.example .env
# preencha as variáveis no .env
dotnet ef database update
dotnet run
```

**Variáveis de ambiente da API (`.env`):**

| Variável | Descrição |
|----------|-----------|
| `DATABASE_URL` | String de conexão PostgreSQL |
| `RESEND_API_KEY` | Chave da API Resend (envio de e-mails) |
| `FRONTEND_URL` | URL do frontend (para CORS e links de e-mail) |
| `RSA_PRIVATE_KEY` | Chave privada RSA em PEM (opcional se usar arquivo) |
| `RSA_PUBLIC_KEY` | Chave pública RSA em PEM (opcional se usar arquivo) |

### UI

```bash
cd alimenta-bem-ui
cp .env.example .env
# preencha VITE_API_BASE_URL no .env
npm install
npm run dev
```

**Variáveis de ambiente da UI (`.env`):**

| Variável | Descrição |
|----------|-----------|
| `VITE_API_BASE_URL` | URL base da API (ex: `http://localhost:5000`) |

---

## Licença

Apache-2.0 — veja [LICENSE](LICENSE).
