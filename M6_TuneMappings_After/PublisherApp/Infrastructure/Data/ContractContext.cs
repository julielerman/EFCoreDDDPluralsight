using ContractBC.ContractAggregate;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class ContractContext : DbContext
{
    public ContractContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContractVersion>().ComplexProperty(v => v.Specs);
        modelBuilder.Entity<ContractVersion>().OwnsMany(v => v.Authors, prop =>
        {
            prop.ToTable("ContractVersion_Authors");
            prop.OwnsOne(a => a.Name);
        });

        modelBuilder.Entity<Contract>().Property(c => c.ContractNumber).HasField("_contractNumber");
        modelBuilder.Entity<Contract>().Property(c => c.DateInitiated).HasField("_initiated");
        modelBuilder.Entity<ContractVersion>().Property("_hasRevisedSpecSet");
        modelBuilder.Entity<ContractVersion>().ToTable("ContractVersions");
    }
}