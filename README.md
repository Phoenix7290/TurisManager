# TurisManager

TurisManager é uma aplicação web desenvolvida em ASP.NET Core Razor Pages para gerenciamento de pacotes turísticos, clientes, reservas e destinos. A aplicação permite criar, editar, visualizar e excluir registros de clientes, pacotes turísticos e reservas, além de gerenciar destinos associados a pacotes.

## Login

Login: admin
Password: 123456
Code Behind em Login.cshtml.cs

## Funcionalidades

- **Gerenciamento de Clientes**:
  - Criação, edição, visualização e exclusão (soft delete) de clientes.
  - Filtro para exibir apenas clientes não excluídos.
- **Gerenciamento de Pacotes Turísticos**:
  - Criação, edição, visualização e exclusão de pacotes turísticos.
  - Associação de múltiplos destinos (cidades) a pacotes turísticos.
  - Status de confirmação para pacotes.
- **Gerenciamento de Reservas**:
  - Criação de reservas associando clientes a pacotes turísticos.
  - Cálculo automático do valor total com base em diárias e valor por diária.
  - Filtros por status (confirmada/pendente) e busca por cliente ou pacote.
- **Gerenciamento de Destinos**:
  - Cadastro de cidades e países como destinos.
  - Relacionamento entre cidades e países (um país pode ter várias cidades).
- **Autenticação**:
  - Login e logout com autenticação baseada em cookies.
  - Redirecionamento automático para a página de login quando não autenticado.
- **Interface de Usuário**:
  - Interface responsiva usando Bootstrap 5.
  - Validações de formulário com jQuery Validation.
  - Feedback visual para ações bem-sucedidas ou erros.

## Tecnologias Utilizadas

- **Backend**: ASP.NET Core 8.0, Entity Framework Core 9.0.6
- **Banco de Dados**: SQL Server (via LocalDB para desenvolvimento)
- **Frontend**: Razor Pages, Bootstrap 5, jQuery
- **Controle de Versão**: Git
- **Containerização**: Docker
- **Outras Dependências**:
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
  - Microsoft.VisualStudio.Azure.Containers.Tools.Targets

## Requisitos

- **Desenvolvimento**:
  - .NET 8.0 SDK
  - Visual Studio 2022 (ou superior) ou outro editor compatível
  - SQL Server LocalDB (para desenvolvimento)
- **Produção**:
  - Docker e Docker Compose
  - Banco de dados SQL Server (ou outro compatível com EF Core)

## Estrutura do Projeto

```
TurisManager/
├── Data/
│   └── TurisManagerContext.cs           # Contexto do Entity Framework
├── Migrations/                         # Migrações do banco de dados
├── Models/                             # Modelos de dados (Cliente, PacoteTuristico, etc.)
├── Pages/                              # Páginas Razor para UI
├── Properties/                         # Configurações de inicialização
├── wwwroot/                            # Arquivos estáticos (CSS, JS)
├── appsettings.json                    # Configurações da aplicação
├── Dockerfile                          # Definição do container Docker
├── TurisManager.csproj                 # Arquivo de projeto .NET
├── .dockerignore                       # Ignora arquivos no build do Docker
├── .gitignore                          # Ignora arquivos no Git
├── LICENSE.txt                         # Licença MIT
└── TurisManager.sln                    # Solução do Visual Studio
```

## Configuração para Desenvolvimento

Pode ser usado Inicialmente com git clone pois há DB, mas caso deseje fazer do zero:

1. **Clonar o Repositório**:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd TurisManager
   ```

2. **Restaurar Dependências**:
   ```bash
   dotnet restore
   ```

3. **Configurar Banco de Dados**:
   - Certifique-se de que o SQL Server LocalDB está instalado.
   - A string de conexão padrão está em `appsettings.json`:
     ```json
     "ConnectionStrings": {
         "TurisManagerContext": "Server=(localdb)\\mssqllocaldb;Database=TurisManager;Trusted_Connection=True;"
     }
     ```
   - Aplique as migrações para criar o banco de dados:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

4. **Executar a Aplicação**:
   ```bash
   dotnet run
   ```
   - Acesse a aplicação em `http://localhost:5106` ou `https://localhost:7152`.

## Agradecimentos

Obrigado ao Professor responsável Rinaldo e meus colegas de classe pela ajuda no projeto.
