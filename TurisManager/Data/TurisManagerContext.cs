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
    }
}