using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Futebol.Models
{
    public class LigaTabajaraContext : DbContext
    {
        public DbSet<Time> Times { get; set; }
        public DbSet<Jogador> Jogadores { get; set; }
        public DbSet<ComissaoTecnica> ComissoesTecnicas { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Estatistica> Estatisticas { get; set; }

        public LigaTabajaraContext() : base("LigaTabajara")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurar exclusão em cascata para Jogadores
            modelBuilder.Entity<Time>()
                .HasMany(t => t.Jogadores)
                .WithRequired(j => j.Time)
                .HasForeignKey(j => j.TimeId)
                .WillCascadeOnDelete(true);

            // Configurar exclusão em cascata para ComissaoTecnica
            modelBuilder.Entity<Time>()
                .HasMany(t => t.ComissaoTecnica)
                .WithRequired(c => c.Time)
                .HasForeignKey(c => c.TimeId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}