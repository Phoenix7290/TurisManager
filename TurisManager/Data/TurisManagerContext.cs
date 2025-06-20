using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TurisManager.Models;

namespace TurisManager.Data
{
    public class TurisManagerContext : DbContext
    {
        public TurisManagerContext(DbContextOptions<TurisManagerContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CidadeDestino> CidadesDestinos { get; set; }
        public DbSet<PaisDestino> PaisesDestinos { get; set; }
        public DbSet<PacoteTuristico> PacotesTuristicos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da relação Cliente -> Reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação PacoteTuristico -> Reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.PacoteTuristico)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.PacoteTuristicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação PaisDestino -> CidadesDestinos
            modelBuilder.Entity<CidadeDestino>()
                .HasOne(c => c.PaisDestino)
                .WithMany(p => p.Cidades)
                .HasForeignKey(c => c.PaisDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação muitos-para-muitos entre PacoteTuristico e CidadeDestino
            modelBuilder.Entity<PacoteTuristico>()
                .HasMany(p => p.Destinos)
                .WithMany(c => c.PacotesTuristicos)
                .UsingEntity(j => j.ToTable("PacoteTuristicoDestinos"));

            // Configurações de propriedades
            modelBuilder.Entity<Reserva>()
                .Property(r => r.ValorTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PacoteTuristico>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            // Configuração para soft delete do Cliente
            modelBuilder.Entity<Cliente>()
                .HasQueryFilter(c => !c.IsDeleted);
        }
    }
}