﻿ using ContractBC.ContractAggregate;
using ContractBC.Enums;
using ContractBC.Services;
using ContractBC.ValueObjects;

namespace ContractBC.UnitTests;

[TestClass]
public class VersionTests
{
    List<Author> _unsignedAuthors;
    Contract _contract;

    public VersionTests()
    {
        _unsignedAuthors = new List<Author> { Author.UnsignedAuthor(
                                              "first", "last", "email", "phone") };
        _contract = new Contract(DateOnly.FromDateTime(DateTime.Now), _unsignedAuthors, "booktitle");
    }

    [TestMethod]
    public void NewVersionHasSpecDefaults()
    {
        var versionattribs = VersionAttributeFactory.Create(Guid.Empty, " ", new List<Author>(),
                                                            ModReason.NewContract, "");
        var version = ContractVersion.CreateNew(versionattribs);
        var defaultSpecs = ContractVersion.GetDefaultSpecs();
        Assert.AreEqual(defaultSpecs, version.Specs);
    }

    [TestMethod]
    public void NewContractVersionValueEqualsNewContract()
    {
        Assert.AreEqual(ModReason.NewContract, _contract.CurrentVersion().ModificationReason);
    }

    [TestMethod]
    public void NewContractCurrentVersionHasCorrectAuthor()
    {
        var nameFromContract =
           _contract.CurrentVersion().Authors.FirstOrDefault().Name.FullName;
        Assert.AreEqual("first last", nameFromContract);
    }

    [TestMethod]
    public void NewContractVersionHasId()
    {
        Assert.AreNotEqual(Guid.Empty, _contract.CurrentVersion().Id);
    }

    [TestMethod]
    public void CurrentVersionOfNewContractHasCorrectAuthorNameAndBookTitle()
    {
        var namefromContract =
              _contract.CurrentVersion().Authors.FirstOrDefault().FullName;
        Assert.AreEqual("first last booktitle", $"{namefromContract} booktitle");
    }

    [TestMethod]
    public void DerivedVersionIdIsProtected()
    {
        var prop = typeof(ContractVersion).GetProperty("Id");
        Assert.IsTrue(prop.SetMethod.IsFamily);
    }

    [TestMethod]
    public void AuthorsCopyIsFilledWithMatchingAuthors()
    {//this tests domain logic, not EF Core interpretation
        var authors = _contract.CurrentVersion().Authors.ToList();
        var authorsCopy = _contract.CurrentVersion().AuthorsCopy().ToList();
        Assert.AreEqual(_contract.CurrentVersion().Authors.Count(), authors.Count());
        CollectionAssert.AreEqual(authors, authorsCopy);
    }

    //[TestMethod]
    //public void SpecSetIsImmutable()
    //{

    //    var defaultSpecs = ContractVersion.GetDefaultSpecs();
    //    var defaultAAUSD = defaultSpecs.AdvanceAmountUSD;
    //    defaultSpecs.AdvanceAmountUSD = 1; //this should not even compile
    //    Assert.AreEqual(defaultAAUSD, defaultSpecs.AdvanceAmountUSD);
    //}
}

