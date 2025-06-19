# UpgradeHub

**Monorepo** para a plataforma UpgradeHub, uma ferramenta colaborativa de sugestões e melhorias de software.

---

## 📂 Estrutura do Repositório

```
UpgradeHub/
├── UpgradeHub.sln              # Solução .NET para backend
├── UpgradeHub.Api/             # Projeto ASP.NET Core Web API
├── UpgradeHub.Domain/          # Camada de domínio (models, entidades)
├── UpgradeHub.Application/     # Camada de aplicação (serviços, casos de uso)
├── UpgradeHub.Infrastructure/  # Camada de infraestrutura (EF Core, repositórios)
├── UpgradeHub.Tests/           # Projeto de testes automatizados (xUnit)
└── frontend/                   # SPA Vue 3 com Vite e Pinia
```

---

## 🚀 Tecnologias

* **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core, xUnit
* **Frontend:** Vue 3, Material UI (Vuetify, Vite, Pinia, Vue Router (Unplugin), Axios
* **Controle de versão:** Git, GitHub
* **Deploy (futuro):** Azure App Service, Azure SQL / PostgreSQL

---

## ⚙️ Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Entity Framework](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
* [Node.js LTS](https://nodejs.org/)
* [Git](https://git-scm.com/)
* [SQL Server Express](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ** Possivelmente irá ser alterado para Postgres futuramente

---

## 📝 Configuração Inicial

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/UpgradeHub.git
   cd UpgradeHub
   ```
2. Crie o arquivo de ambiente para o frontend:

   ```bash
   cd frontend
   cp .env.example .env
   ```

   Edite `.env` com a URL da API:

   ```env
   VITE_API_BASE_URL=http://localhost:5000/api
   ```
3. Instale dependências e rode o frontend:

   ```bash
   npm install
   npm run dev
   ```

---

## 📦 Backend

### 1. Abrir no Visual Studio

* Abra a solução `UpgradeHub.sln` no Visual Studio 2022+.

### 2. Restaurar, compilar e Gerar o banco de dados

* No Visual Studio: **Build Solution** (Ctrl+Shift+B)
* No Visual Studio: > Ferramentas > Linha de Comando > Powershell do desenvolvedor rodar o script: dotnet ef migrations add InitialCreate --project "SuggestionHub.Infrastructure" --startup-project "SuggestionHub.API" --context AppDbContext

### 3. Executar a API

* Defina `UpgradeHub.Api` como projeto inicial e pressione **F5**.
* Acesse Swagger UI em `https://localhost:5001/swagger/index.html`.

### 4. Testes automatizados

* Abra **Test Explorer** no Visual Studio e clique em **Run All**.

-- Ainda não implementado

---

## 🖥️ Frontend

### 1. Instalar dependências

```bash
cd frontend
yarn install
```

### 2. Estrutura de Pastas

```
frontend/
├── public/               # arquivos estáticos (favicon, index.html)
├── src/
│   ├── assets/           # imagens, fontes, css
│   ├── components/       # componentes Vue reutilizáveis
│   ├── pages/            # páginas principais
│   ├── router/           # configuração das rotas
│   ├── store/            # Pinia para estado global
│   ├── services/         # serviços para comunicação com API (axios)
│   └── App.vue           # componente raiz
├── .env.example          # exemplo de variáveis de ambiente
└── vite.config.js        # config do Vite (proxy para API)
```

### 3. Executar em modo de desenvolvimento

```bash
yarn dev
```

* Acesse `http://localhost:3000`

### 5. Build para produção

```bash
yarn build
```

* Os arquivos ficam em `frontend/dist`.

---

## 🤝 Contribuição

1. Fork e clone.
2. Branch de feature.
3. Commit e push.
4. Pull Request.

---

## 📄 Licença

MIT License. Veja o arquivo [LICENSE](LICENSE).
