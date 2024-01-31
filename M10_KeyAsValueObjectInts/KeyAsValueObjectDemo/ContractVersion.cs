using KeyAsValueObjectDemo.SharedKernel;
using KeyAsValueObjectDemoInts.ValueObjects;

namespace KeyAsValueObjectDemoInts;

public class ContractVersion:BaseEntity
{
    public ContractVersionId Id { get; init; }
    public ContractId ContractId { get; init; }
    public string WorkingTitle { get; init; }
    public DateTime DateCreated { get; private set; }

    public ContractVersion(ContractId contractId, string title)
    {
       // Id = new ContractVersionId(Guid.NewGuid());
        WorkingTitle = title;
        ContractId = contractId;

    }
    private ContractVersion()
    {
        
    }
}

