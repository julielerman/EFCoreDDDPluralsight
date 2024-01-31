namespace KeyAsValueObjectDemoGuids.ValueObjects;

public record ContractId
{
    public ContractId(Guid value) => Value = value;
    public Guid Value { get; init; }
}

public record ContractVersionId
{
    public ContractVersionId(Guid value) => Value = value;
    public Guid Value { get; init; }
}



