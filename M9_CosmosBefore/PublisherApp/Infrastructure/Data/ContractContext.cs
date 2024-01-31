using ContractBC.ContractAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
namespace Infrastructure.Data;

public class ContractContext : DbContext
{
    public ContractContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>().Property(c => c.ContractNumber).HasField("_contractNumber");
        modelBuilder.Entity<Contract>().Property(c => c.DateInitiated).HasField("_initiated");
        new ContractVersionEntityTypeConfiguration().Configure(modelBuilder.Entity<ContractVersion>());
    }

    public class ContractVersionEntityTypeConfiguration: 
        IEntityTypeConfiguration<ContractVersion>
    {
        public void Configure(EntityTypeBuilder<ContractVersion> builder)
        {
            builder.ComplexProperty(v => v.Specs);
            builder.OwnsMany(v => v.Authors, prop =>
            {
                prop.ToTable("ContractVersion_Authors");
                prop.OwnsOne(a => a.Name);
            });
            builder.Property("_hasRevisedSpecSet");
            builder.ToTable("ContractVersions");
            builder.Property(v => v.Id).ValueGeneratedNever();

        }
    }

   


}