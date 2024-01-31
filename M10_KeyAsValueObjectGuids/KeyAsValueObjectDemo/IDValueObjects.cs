namespace KeyAsValueObjectDemoGuids.ValueObjects;

public class ContractId
{
    public ContractId(Guid value) => Value = value;
    public Guid Value { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is ContractId value &&
               Value.Equals(value.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}

public class ContractVersionId
{
    public ContractVersionId(Guid value) => Value = value;
    public Guid Value { get; private set; }

    public override bool Equals(object? obj)
    {
        return obj is ContractVersionId value &&
               Value.Equals(value.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}



