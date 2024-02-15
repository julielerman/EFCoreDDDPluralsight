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
        modelBuilder.Entity<ContractVersion>().OwnsMany(v=> v.Authors).OwnsOne(a=>a.Name);
    }
}