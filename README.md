# Futebol

# Gerenciamento de Comissão Técnica

Este projeto é uma aplicação ASP.NET MVC que permite o gerenciamento da comissão técnica de um time de futebol. O sistema permite o cadastro, edição, visualização e exclusão de membros da comissão técnica, com a possibilidade de realizar pesquisas filtrando por nome e cargo.

## Funcionalidades

- **Cadastro de Comissão Técnica**: Permite adicionar membros da comissão técnica, incluindo nome, cargo, data de nascimento, e time ao qual pertence.
- **Edição de Comissão Técnica**: Possibilita a edição dos dados de um membro da comissão técnica.
- **Exclusão de Comissão Técnica**: Permite excluir membros da comissão técnica do banco de dados.
- **Busca de Comissão Técnica**: Permite buscar membros da comissão técnica filtrando por nome e cargo (goleiro, zagueiro, etc).
  
## Tecnologias Utilizadas

- **ASP.NET MVC**: Framework principal para o desenvolvimento da aplicação.
- **Entity Framework**: Utilizado para interação com o banco de dados.
- **SQL Server**: Banco de dados utilizado para persistência dos dados.
- **HTML/CSS/JavaScript**: Para o frontend da aplicação.
- **Bootstrap**: Framework CSS para uma interface responsiva e moderna.

## Pré-requisitos

Antes de rodar o projeto, é necessário ter as seguintes ferramentas instaladas:

- [Visual Studio](https://visualstudio.microsoft.com/) (com a carga de trabalho "ASP.NET and web development")
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) ou uma alternativa compatível (como o SQL Server Express).
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (opcional, para gerenciar o banco de dados)

## Como Rodar o Projeto

### 1. Clone o Repositório

### 2. Configure o Banco de Dados

1. Crie um banco de dados no SQL Server.
2. Atualize a string de conexão no arquivo `Web.config` com os detalhes do seu banco de dados.

<!--
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=SEU_SERVIDOR;Database=NomeDoBancoDeDados;Integrated Security=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
-->

### 3. Execute as Migrations (se necessário)

<!--
Se o projeto usar Entity Framework e você ainda não tiver as tabelas criadas, execute as migrations para criar o banco de dados:

```bash
Update-Database
-->
## Rodando o Projeto

No Visual Studio, clique em **Iniciar** ou pressione `F5` para rodar a aplicação localmente. O projeto será iniciado em seu navegador padrão, e você poderá acessar a interface web para gerenciar a comissão técnica.

## Estrutura do Projeto

- **Controllers**: Contém as ações para manipular os dados da comissão técnica.
  - `ComissaoTecnicaController.cs`: Ações para exibir, editar, excluir e buscar membros da comissão técnica.

- **Models**: Contém os modelos de dados.
  - `ComissaoTecnica.cs`: Representa os membros da comissão técnica.
  - `Cargo.cs`: Enum que define os cargos da comissão técnica.

- **Views**: Contém as páginas de interface com o usuário.
  - `Index.cshtml`: Página de listagem e filtro dos membros da comissão técnica.
  - `Create.cshtml`, `Edit.cshtml`, `Details.cshtml`, `Delete.cshtml`: Páginas para cada operação CRUD.

## Contribuindo

1. Faça o **fork** do repositório.
2. Crie uma nova **branch** (`git checkout -b feature-nome-da-feature`).
3. Faça as alterações desejadas e **commite** (`git commit -am 'Adicionando nova funcionalidade'`).
4. **Push** para a sua branch (`git push origin feature-nome-da-feature`).
5. Abra um **Pull Request**.

## Licença
Este projeto está licenciado sob a MIT License.

### Descrição dos Componentes:

1. **Objetivo**: O projeto é voltado para gerenciar uma comissão técnica de futebol.
2. **Tecnologias**: Enumera as tecnologias usadas, como ASP.NET MVC, Entity Framework, e SQL Server.
3. **Instruções de Configuração**: Fornece uma visão geral sobre como configurar o banco de dados e rodar o projeto localmente.
4. **Estrutura do Projeto**: Explica como o projeto está organizado para facilitar o entendimento de quem for trabalhar nele.
5. **Como Contribuir**: Instruções para quem quiser contribuir no projeto.
