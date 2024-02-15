using System.Text.Json;

namespace IntegrationTests;

public static class DefaultSpecsFactory
{
    public static SpecificationSet Create()
    {
        //read from json file
        string fileName = "DefaultSpecificationSet.json";
        string jsonString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<SpecificationSet>(jsonString)!;
    }
}

