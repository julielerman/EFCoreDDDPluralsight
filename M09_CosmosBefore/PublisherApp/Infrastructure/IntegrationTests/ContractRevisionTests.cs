using ContractBC.ContractAggregate;
using ContractBC.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Text.Json;

namespace IntegrationTests;

[TestClass]
public class ContractRevisionTests
{ 
    Contract _contract = new Contract(
       DateOnly.FromDateTime(DateTime.Today),
       new List<Author> { Author.UnsignedAuthor("first", "last", "email", "phone") }, "booktitle"
       );
    ContractContext _context;

    public ContractRevisionTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ContractContext>();
        optionsBuilder.UseSqlServer(
             "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=PubContractIntegrationTests");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        var _options = optionsBuilder.Options;
        _context = new ContractContext(_options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    private JsonSerializerOptions CustomJsonOptions()
    {
        var options = new JsonSerializerOptions() { WriteIndented = true };
        options.Converters.Add(new CustomDateTimeConverter("yyyy-MM-ddTHH:mm:ss"));
        options.Converters.Add(new CustomDecimalConverter("F"));
        return options;
    }
[TestMethod]
    public void AuthorsCopyContainsSameAuthorValues()
    {
        _context.Contracts.Add(_contract);
        var authorsCopy=_contract.CurrentVersion().AuthorsCopy();
      
        CollectionAssert.AreEqual(_contract.CurrentVersion().Authors.ToList(), authorsCopy.ToList());
    }
    [TestMethod]
    public void AuthorsCopyCanBeTrackedInNewContractVersion()
    {
        _context.Contracts.Add(_contract);
        var authorsCopy = _contract.CurrentVersion().AuthorsCopy();
        _contract.CreateRevisionUsingSameSpecs(ContractBC.Enums.ModReason.Other, "abc",
                                  _contract.CurrentVersion().WorkingTitle, authorsCopy.ToList(), null);
        _context.ChangeTracker.DetectChanges();      
        var entries=_context.ChangeTracker.Entries<Author>().ToList();
        Assert.AreNotEqual(entries[0],entries[1]);
    }

    [TestMethod]
    public void CanCopySpecSetToDifferentVersion()
    {
        _context.Contracts.Add(_contract);
        var v1= _contract.CurrentVersion();
        _context.SaveChanges();
        _contract.CreateRevisionUsingNewSpecs(ContractBC.Enums.ModReason.Other, "abc",
                       _contract.CurrentVersion().WorkingTitle, 
                       new List<Author> { Author.UnsignedAuthor("xfirst", "last", "email", "phone") },
                                  null, v1.Specs); //we're not MOVING specs, it becomes a copy
        _context.ChangeTracker.DetectChanges();
        Assert.AreEqual(_contract.CurrentVersion().Specs,v1.Specs);

    }
    [TestMethod]
    public void CanMoveAuthorsToDifferentVersion()
    {
        _context.Contracts.Add(_contract);
        var v1 = _contract.CurrentVersion();
        _contract.CreateRevisionUsingNewSpecs(ContractBC.Enums.ModReason.Other, "abc",
                       _contract.CurrentVersion().WorkingTitle, _contract.CurrentVersion().Authors.ToList(),
                                  null, v1.Specs);
        Assert.AreEqual(2, _contract.Versions.Count());
    }

    [TestMethod]
    public void PersistedContractWithRevisonReturnsTwoVersions()
    {
        CreateNewContractAndAddRevision();
        //added in for debugging a hard problem
        //_context.ChangeTracker.DetectChanges();
        //
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var contractFromDB = _context.Contracts.Include(c => c.Versions)
            .FirstOrDefault();
        Assert.AreEqual(2, contractFromDB.Versions.Count());
    }
    
    [TestMethod]
    public void CanUseFilteredInclude()
    {
        CreateNewContractAndAddRevision();
        var cid = _contract.CurrentVersionId;
        //added in for debugging a hard problem
        _context.ChangeTracker.DetectChanges();
        //
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var contractFromDB = _context.Contracts.Include(c => c.Versions.Where(v => v.Id == cid))
            .FirstOrDefault();
        Assert.AreEqual(1, contractFromDB.Versions.Count());
    }
   
    [TestMethod]
    public void RevisonUsingSameSpecsStoresHasRevisedAsFalse()
    {
        CreateNewContractAndAddRevision();
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var value = _context.Database.SqlQuery<bool>
          ($"SELECT TOP 1 [_hasRevisedSpecSet] as Value FROM ContractVersions Order by DateCreated Desc")
          .FirstOrDefault();
        Assert.AreEqual(false, value);
    } 
    [TestMethod]
    public void RevisonUsingNewSpecsStoresHasRevisedAsTrue()
    {
        CreateNewContractAndAddRevisionWithNewSpecs();
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
         var value = _context.Database.SqlQuery<bool>
        ($"SELECT TOP 1 [_hasRevisedSpecSet] as Value FROM ContractVersions Order by DateCreated Desc")
        .FirstOrDefault();
        Assert.AreEqual(true, value);
    }

    [TestMethod]
    public void PersistedContractWithRevisonAndSameAuthorsDuplicatesAuthorInDatabase()
    {
        var authorsv1 = _contract.CurrentVersion().Authors.ToList();
        CreateNewContractAndAddRevision();
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var contractFromDB = _context.Contracts
            .Select(c => new
            {
                c.CurrentVersionId,
                current = c.Versions.FirstOrDefault(v => v.Id == c.CurrentVersionId)
            }).FirstOrDefault();
        var authorsv2 = contractFromDB.current.Authors.ToList();
        Assert.AreEqual(JsonSerializer.Serialize(authorsv1, CustomJsonOptions()),
            JsonSerializer.Serialize(authorsv2, CustomJsonOptions()));
    }

    private void CreateNewContractAndAddRevision()
    {
        _context.Contracts.Add(_contract);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var contractFromDB = _context.Contracts.Include(c => c.Versions).FirstOrDefault();
        var v1 = contractFromDB.CurrentVersion();
        contractFromDB.CreateRevisionUsingSameSpecs
            (ContractBC.Enums.ModReason.Other, "abc",
             v1.WorkingTitle, v1.AuthorsCopy().ToList(), null);
    }

    private void CreateNewContractAndAddRevisionWithNewSpecs()  
    {
        _context.Contracts.Add(_contract);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        var contractFromDB = _context.Contracts.Include(c => c.Versions).FirstOrDefault();
        var v1 = contractFromDB.CurrentVersion();
        contractFromDB.CreateRevisionUsingNewSpecs
            (ContractBC.Enums.ModReason.Other, "abc", v1.WorkingTitle,
             v1.AuthorsCopy().ToList(), null, DefaultSpecsFactory.Create());
    }

 

}
