using KeyAsValueObjectDemo.SharedKernel;
using KeyAsValueObjectDemoInts.ValueObjects;

namespace KeyAsValueObjectDemoInts;

public class Contract:BaseEntity
{
    private Contract() { }
    public Contract(string workingTitle)
    {
        //Id = new ContractId(Guid.NewGuid());
        AddVersion(new ContractVersion(Id, workingTitle));
        ContractNumber = $"{Guid.NewGuid()}_{workingTitle}";
    }
    public ContractId Id { get; init; }
    public string ContractNumber { get; init; }
    public IEnumerable<ContractVersion> Versions => _versions.ToList();
    private readonly List<ContractVersion> _versions = new List<ContractVersion>();
    private void AddVersion(ContractVersion version)
    {
        _versions.Add(version);
    }
}

