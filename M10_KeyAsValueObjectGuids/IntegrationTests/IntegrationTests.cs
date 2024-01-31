
using IntegrationTests;
using KeyAsValueObjectDemoGuids;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace KeyAsValueObjectDemo.IntegrationTests
{
    [TestClass]
    public class IntegrationTests
    {
        Contract _contract = new Contract("A New Book");

        ContractContext _context;

        public IntegrationTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContractContext>();
            optionsBuilder.UseSqlServer(
                 "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=KeyAsValueObjectDemo");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
            var _options = optionsBuilder.Options;
            _context = new ContractContext(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [TestMethod]
        public void NewContractHasNonEmptyId()
        {
            _context.Contracts.Add(new Contract("A New Book"));
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var contractFromDB = _context.Contracts.FirstOrDefault();
            Assert.AreNotEqual(Guid.Empty, contractFromDB.Id.Value);
        }

        [TestMethod]
        public void NewContractStoresCorrectId()
        {
            var c = new Contract("A New Book");
            var assignedId = c.Id.Value;
            _context.Contracts.Add(c);

            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var contractFromDB = _context.Contracts.FirstOrDefault();
            Assert.AreNotEqual(Guid.Empty, contractFromDB.Id.Value);
            Assert.AreEqual(assignedId, contractFromDB.Id.Value);
        }

        [TestMethod]
        public void NewContractStoresCorrectIdInItsVersion()
        {
          
            _context.Contracts.Add(_contract);
            _context.SaveChanges();
            var assignedFKId = _contract.Versions.First().ContractId.Value;
            _context.ChangeTracker.Clear();
            var contractFromDB = _context.Contracts.Include(c => c.Versions).FirstOrDefault();
            Assert.AreEqual(assignedFKId, contractFromDB.Versions.First().ContractId.Value);
        }

        [TestMethod]
        public void CanQueryByContractIdType()
        { 
            
        _context.Contracts.Add(_contract);
            _context.SaveChanges();
            var assignedId = _contract.Id;
            _context.ChangeTracker.Clear();
            var contractFromDB = _context.Contracts.FirstOrDefault(c=>c.Id== assignedId);
            Assert.AreEqual(assignedId.Value, contractFromDB.Id.Value) ;
    
            }




    [TestMethod]
        public void NewContractValuesPersistedandRetrieved()
        {
            _context.Contracts.Add(_contract);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            var contractFromDB = _context.Contracts.Include(c => c.Versions).FirstOrDefault();
           //to force the test to fail, you can make the following modification, changing one of the contract instances
            // contractFromDB.FinalVersionSignedByAllParties();
            var expected = JsonSerializer.Serialize(_contract, CustomJsonOptions());
            var actual = JsonSerializer.Serialize(contractFromDB, CustomJsonOptions());
            Assert.AreEqual(expected, actual);
          
        }

        private JsonSerializerOptions CustomJsonOptions()
        {
            var options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new CustomDateTimeConverter("yyyy-MM-ddTHH:mm:ss"));
             options.Converters.Add(new CustomDecimalConverter("F"));
            return options;

        }

      
    }
}