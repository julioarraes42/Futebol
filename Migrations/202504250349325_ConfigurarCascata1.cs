namespace Futebol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigurarCascata1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times");
            DropForeignKey("dbo.Jogadors", "TimeId", "dbo.Times");
            DropForeignKey("dbo.Estatisticas", "JogadorId", "dbo.Jogadors");
            DropForeignKey("dbo.Estatisticas", "PartidaId", "dbo.Partidas");
            DropForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times");
            DropForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times");

            // Configurar exclusão em cascata para TimeCasaId
            AddForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times", "Id", cascadeDelete: true);

            // Configurar TimeForaId sem exclusão em cascata
            AddForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times", "Id", cascadeDelete: false);

            // Configurar Estatisticas sem exclusão em cascata
            AddForeignKey("dbo.Estatisticas", "PartidaId", "dbo.Partidas", "Id", cascadeDelete: false);

            AddForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Jogadors", "TimeId", "dbo.Times", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Estatisticas", "JogadorId", "dbo.Jogadors", "Id", cascadeDelete: true);
        }


        public override void Down()
        {
            DropForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times");
            DropForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times");
            DropForeignKey("dbo.Estatisticas", "PartidaId", "dbo.Partidas");
            DropForeignKey("dbo.Estatisticas", "JogadorId", "dbo.Jogadors");
            DropForeignKey("dbo.Jogadors", "TimeId", "dbo.Times");
            DropForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times");
            AddForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times", "Id");
            AddForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times", "Id");
            AddForeignKey("dbo.Estatisticas", "PartidaId", "dbo.Partidas", "Id");
            AddForeignKey("dbo.Estatisticas", "JogadorId", "dbo.Jogadors", "Id");
            AddForeignKey("dbo.Jogadors", "TimeId", "dbo.Times", "Id");
            AddForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times", "Id");
        }
    }
}
