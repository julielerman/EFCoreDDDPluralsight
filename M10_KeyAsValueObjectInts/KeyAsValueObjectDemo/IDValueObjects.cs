namespace KeyAsValueObjectDemoInts.ValueObjects;

public record ContractId
{
    public ContractId(int value) => Value = value;
    public int Value { get; init; }
}

public record ContractVersionId
{
    public ContractVersionId(int value) => Value = value;
    public int Value { get; init; }
}



