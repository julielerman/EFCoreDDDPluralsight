using ContractBC.ContractAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublisherSystem.SharedKernel;
using System.Reflection.Emit;
namespace Infrastructure.Data;

public class ContractContext : DbContext
{
    private readonly IMediator _mediator;


    public ContractContext(DbContextOptions options) : base(options)
    {
        SavedChanges += ContractContext_SavedChanges; 
    }

    public ContractContext(DbContextOptions<ContractContext> options, IMediator mediator)
       : this(options)
    {
        _mediator = mediator;
    }
    private void ContractContext_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        // ignore events if no dispatcher provided
        if (_mediator == null) return;

        var entitiesWithEvents = ChangeTracker
            .Entries()
            .Select(e => e.Entity as BaseEntity<Guid>)
            .Where(e => e?.Events != null && e.Events.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.Events.ToArray();
            entity.Events.Clear();
            foreach (var domainEvent in events)
            {
                _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }
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