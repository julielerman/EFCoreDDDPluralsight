using ContractBC.Enums;
using ContractBC.Services;
using ContractBC.ValueObjects;
using PublisherSystem.SharedKernel;

namespace ContractBC.ContractAggregate;

public class Contract : BaseEntity<Guid>
{
    public Contract(DateOnly initDate, List<Author> authors, string workingTitle)
    {
        _initiated = initDate;
        Id = Guid.NewGuid();
        var baseattribs =
            VersionAttributeFactory.Create(Id, workingTitle, authors, 
                                           ModReason.NewContract, "New Contract");
        ContractVersion version= ContractVersion.CreateNew(baseattribs);
        _contractNumber = GenerateContractNumber(version);
        AddVersion(version);
    }

    private void AddVersion(ContractVersion version)
    {
        _versions.Add(version);
        CurrentVersionId = version.Id;
    }

    public string ContractNumber => _contractNumber;
    public DateOnly DateInitiated => _initiated;
    public Guid CurrentVersionId { get; private set; }
    public Guid FinalVersionId { get; private set; }
    public bool Completed { get; private set; }
    public DateOnly CompletedDate { get; private set; }
    public DateOnly Fullfilled { get; private set; }
    public IEnumerable<ContractVersion> Versions => _versions.ToList();

    private string _contractNumber;
    private DateOnly _initiated;
    private readonly List<ContractVersion> _versions = new List<ContractVersion>();

    public void CreateRevisionUsingSameSpecs
        (ModReason modReason, string modDescription, string title, List<Author> authors,
         DateOnly? customDeadline)
    {
        CreateRevision(modReason, modDescription, title, authors, customDeadline,
                       CurrentVersion().Specs, true);
    }

    public void CreateRevisionUsingNewSpecs
        (ModReason modReason, string modDescription, string title, List<Author> authors, 
         DateOnly? customDeadline, SpecificationSet specs)
    {
        CreateRevision(modReason, modDescription, title, authors, customDeadline, specs, false);
    }

    private void CreateRevision
        (ModReason modReason, string modDescription, string title, List<Author> authors,
         DateOnly? customDeadline, SpecificationSet specs, bool sameSpecs)
    {
        var baseattribs = 
            VersionAttributeFactory.Create(Id, title, authors, modReason, modDescription);
        ContractVersion revision;
        if (customDeadline == null)
        {
            revision = ContractVersion.CreateRevision(baseattribs, specs, !sameSpecs);
        }
        else
        {
            revision = ContractVersion.CreateRevisionWithCustomDeadline(
                baseattribs, specs, !sameSpecs, (DateOnly)customDeadline);
        }

        AddVersion(revision);
    }

    public ContractVersion GetVersion(Guid versionId)
    {
        return Versions.SingleOrDefault(v => v.Id == versionId);
    }

    public ContractVersion CurrentVersion()
    {
        return Versions.Single(v => v.Id == CurrentVersionId);
    }

    public void FinalVersionSignedByAllParties()
    {
        Completed = true;
        CompletedDate = DateOnly.FromDateTime(DateTime.Now);
        FinalVersionId = CurrentVersionId;
    }

    public void CurrentVersionAcceptedVerbally()
    {
        CurrentVersion().VersionAccepted();
    }

    public void AddAuthor(Author author, ContractVersion version)
    {
        version.AddAuthor(author);
    }

    private string GenerateContractNumber(ContractVersion version)
    {
        var date = DateInitiated.ToShortDateString();
        var authorInits =
            new string(version.Authors.SelectMany(a => a.Name.ComplexInitials).ToArray());
        return $"{date}_{authorInits}";
    }

  






}
