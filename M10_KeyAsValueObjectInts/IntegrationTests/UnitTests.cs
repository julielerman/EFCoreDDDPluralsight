using KeyAsValueObjectDemoInts;

namespace KeyAsValueObjectDemo.IntegrationTests
{
    [TestClass]
    public class UnitTests
    {
       [TestMethod]
        public void NewContractHasNullId()
        {
            var contract = new Contract("A New Book");
            Assert.IsNull(contract.Id);
        }

    }
}