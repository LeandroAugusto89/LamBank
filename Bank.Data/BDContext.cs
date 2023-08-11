using Bank.Model;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data
{
    public class BDContext : DbContext
    {
        public BDContext()
        {
        }

        public BDContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Agencia> Agencia { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Conta> Conta { get; set; }
        public virtual DbSet<TipoOperacao> TipoOperacao { get; set; }
        public virtual DbSet<Transacao> Transacao { get; set; }
        public virtual DbSet<Parametro> Parametro { get; set; }
        public virtual DbSet<Lancamento> Lancamento { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SQLServerDB_BANK"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agencia>()
                .HasMany(a => a.Conta)
                .WithOne(c => c.Agencia)
                .HasForeignKey(c => c.CodigoAgencia);

            modelBuilder.Entity<Cliente>()
                .HasMany(cl => cl.Conta)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.IdCliente);

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Lancamento)
                .WithOne(t => t.Conta)
                .HasForeignKey(c => c.IdConta);

            modelBuilder.Entity<TipoOperacao>()
                .HasMany(ti => ti.Transacao)
                .WithOne(t => t.TipoOperacao)
                .HasForeignKey(t => t.IdTipoOperacao);

            modelBuilder.Entity<Transacao>()
                .HasMany(ti => ti.Lancamento)
                .WithOne(t => t.Transacao)
                .HasForeignKey(t => t.IdTransacao);






            //modelBuilder.Entity<T2Conta>()
            //    .HasMany(c => c.Saldo)
            //    .WithOne(s => s.Conta)
            //    .HasForeignKey(c => c.IdConta);

            //modelBuilder.Entity<T2Conta>()
            //    .HasMany(c => c.TransacaoCredito)
            //    .WithOne(t => t.ContaCredito)
            //    .HasForeignKey(c => c.IdContaCredito)
            //    .IsRequired(false);

            //modelBuilder.Entity<T2Conta>()
            //    .HasMany(c => c.TransacaoDebito)
            //    .WithOne(t => t.ContaDebito)
            //    .HasForeignKey(c => c.IdContaDebito)
            //    .IsRequired(false);

            //modelBuilder.Entity<Saldo>()
            //    .HasOne(s => s.Transacao)
            //    .WithMany(t => t.Saldos)
            //    .HasForeignKey(s => s.IdTransacao);

        }
    }
}
