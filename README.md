# AlimentaBem

> Plataforma de gestão de doações de alimentos que conecta cidadãos, organizações sociais e administradores.

---

## O que é o AlimentaBem?

O **AlimentaBem** nasceu da necessidade de organizar e acompanhar doações de alimentos de forma simples e transparente. Muitas vezes doações se perdem por falta de comunicação entre quem quer ajudar e quem precisa receber.

A plataforma resolve isso conectando três perfis:

- **Cidadão** — faz login, registra doações de alimentos e acompanha o status de cada uma em tempo real.
- **Organização** — instituições como igrejas, ONGs e escolas cadastram suas necessidades e gerenciam a fila de doações recebidas.
- **Administrador** — gerencia usuários, cargos, pessoas físicas, organizações e todo o fluxo operacional da plataforma.

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
├── alimenta-bem-api/   # Backend — .NET 8 + FastEndpoints + EF Core
└── alimenta-bem-ui/    # Frontend — React 18 + Vite + Ant Design
```

Cada pasta possui seu próprio README com instruções detalhadas de configuração e execução:

- [alimenta-bem-api/README.md](alimenta-bem-api/README.md)
- [alimenta-bem-ui/README.md](alimenta-bem-ui/README.md)

---

## Tecnologias

| Camada | Stack |
|--------|-------|
| Backend | .NET 8, FastEndpoints, Entity Framework Core, SQL Server |
| Frontend | React 18, Vite, Ant Design, Axios |
| Auth | JWT (RS256), BCrypt |
| Banco | SQL Server com migrations via EF Core |

---

## Início rápido

### Pré-requisitos

- .NET 8 SDK
- Node.js 18+
- SQL Server (local ou Docker)

### API

```bash
cd alimenta-bem-api
cp appsettings.example.json appsettings.json
# preencha as variáveis em appsettings.json
dotnet ef database update
dotnet run
```

### UI

```bash
cd alimenta-bem-ui
cp .env.example .env
# preencha VITE_API_URL no .env
npm install
npm run dev
```

---

## Licença

Apache-2.0 — veja [LICENSE](LICENSE).
