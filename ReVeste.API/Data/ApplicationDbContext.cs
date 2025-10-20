using Microsoft.EntityFrameworkCore;
using ReVeste.API.Models;

namespace ReVeste.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Aposta> Apostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aposta>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Apostas)
                .HasForeignKey(a => a.UsuarioId);
        }
    }
}

