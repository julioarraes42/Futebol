namespace Futebol.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustePartida : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComissaoTecnicas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Cargo = c.Int(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Times",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Cidade = c.String(),
                        Estado = c.String(),
                        AnoFundacao = c.Int(nullable: false),
                        Estadio = c.String(),
                        CapacidadeEstadio = c.Int(nullable: false),
                        CorUniformePrimaria = c.String(),
                        CorUniformeSecundaria = c.String(),
                        AptoParaLiga = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Jogadors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        DataNascimento = c.DateTime(nullable: false),
                        Nacionalidade = c.String(),
                        Posicao = c.Int(nullable: false),
                        NumeroCamisa = c.Int(nullable: false),
                        Altura = c.Single(nullable: false),
                        Peso = c.Single(nullable: false),
                        PePreferido = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Estatisticas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JogadorId = c.Int(nullable: false),
                        PartidaId = c.Int(nullable: false),
                        Gols = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jogadors", t => t.JogadorId)
                .ForeignKey("dbo.Partidas", t => t.PartidaId)
                .Index(t => t.JogadorId)
                .Index(t => t.PartidaId);
            
            CreateTable(
                "dbo.Partidas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Rodada = c.Int(nullable: false),
                        TimeCasaId = c.Int(nullable: false),
                        TimeForaId = c.Int(nullable: false),
                        GolsCasa = c.Int(),
                        GolsFora = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Times", t => t.TimeCasaId)
                .ForeignKey("dbo.Times", t => t.TimeForaId)
                .Index(t => t.TimeCasaId)
                .Index(t => t.TimeForaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Partidas", "TimeForaId", "dbo.Times");
            DropForeignKey("dbo.Partidas", "TimeCasaId", "dbo.Times");
            DropForeignKey("dbo.Estatisticas", "PartidaId", "dbo.Partidas");
            DropForeignKey("dbo.Estatisticas", "JogadorId", "dbo.Jogadors");
            DropForeignKey("dbo.Jogadors", "TimeId", "dbo.Times");
            DropForeignKey("dbo.ComissaoTecnicas", "TimeId", "dbo.Times");
            DropIndex("dbo.Partidas", new[] { "TimeForaId" });
            DropIndex("dbo.Partidas", new[] { "TimeCasaId" });
            DropIndex("dbo.Estatisticas", new[] { "PartidaId" });
            DropIndex("dbo.Estatisticas", new[] { "JogadorId" });
            DropIndex("dbo.Jogadors", new[] { "TimeId" });
            DropIndex("dbo.ComissaoTecnicas", new[] { "TimeId" });
            DropTable("dbo.Partidas");
            DropTable("dbo.Estatisticas");
            DropTable("dbo.Jogadors");
            DropTable("dbo.Times");
            DropTable("dbo.ComissaoTecnicas");
        }
    }
}
