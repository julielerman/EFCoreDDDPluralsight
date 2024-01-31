using KeyAsValueObjectDemoInts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KeyAsValueObjectDemoInts;

public class ContractContext : DbContext
{
    public ContractContext(DbContextOptions options) : base(options)
    {    }

    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>().Property(c => c.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ContractVersion>().Property(cv => cv.Id).ValueGeneratedOnAdd();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
             "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=KeyAsValueObjectDemo"); ;
    }
  
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<ContractId>()
            .HaveConversion<ContractIdConverter>();
        configurationBuilder.Properties<ContractVersionId>()
            .HaveConversion<ContractVersionIdConverter>();
    }

    private class ContractIdConverter : ValueConverter<ContractId, int>
    {
        public ContractIdConverter()
            : base(v => v.Value, v => new(v))
        { }
    }

    private class ContractVersionIdConverter : ValueConverter<ContractVersionId, int>
    {
        public ContractVersionIdConverter()
            : base(v => v.Value, v => new(v))
        { }
    }
}
