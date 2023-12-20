using FinistBusinessLogic.Controllers;
using FinistBusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace FinistBusinessLogic.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(ConfigJSON.GetConfig().GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "adminpack")
            .HasPostgresExtension("pgagent", "pgagent");

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("bankaccounts_pkey");

            entity.ToTable("bankaccounts");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("accountnumber");
            entity.Property(e => e.AccountType).HasColumnName("accounttype");
            entity.Property(e => e.ClientPhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("clientphonenumber");

            entity.HasOne(d => d.ClientPhoneNumberNavigation).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.ClientPhoneNumber)
                .HasConstraintName("bankaccounts_clientphonenumber_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.PhoneNumber).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .HasColumnName("phonenumber");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .HasColumnName("middlename");
            entity.Property(e => e.Pass).HasColumnName("pass");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
