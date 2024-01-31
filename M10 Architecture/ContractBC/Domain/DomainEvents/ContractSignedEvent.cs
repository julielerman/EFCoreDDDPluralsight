using ContractBC.ContractAggregate;
using PublisherSystem.SharedKernel;
using PublisherSystem.SharedKernel.DTOs;

namespace ContractBC.Events;

public class ContractSignedEvent : BaseDomainEvent
{
    public ContractSignedEvent(ContractVersion signedVersion, DateTime completionDate)
    {
        ContractId = signedVersion.ContractId;
        Authors = signedVersion.Authors
            .Select(a => new AuthorDTO(a.Name.FirstName, a.Name.LastName,
                                       a.Email, a.Phone, a.Signed, a.SignedAuthorId))
            .ToList();
        Title = signedVersion.WorkingTitle;
        SignedDate = completionDate;
    }
    public Guid Id { get;  init; } = Guid.NewGuid();
    public Guid ContractId { get; init; }
    public List<AuthorDTO> Authors { get; init; } = new List<AuthorDTO>();
    public string Title { get; init; }
    public DateTime SignedDate { get; init; }
}
