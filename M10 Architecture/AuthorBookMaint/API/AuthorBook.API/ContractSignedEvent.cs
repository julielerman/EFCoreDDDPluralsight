using PublisherSystem.SharedKernel;
using PublisherSystem.SharedKernel.DTOs;

namespace AuthorBook.API;

public class ContractSignedEventMessage : BaseDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ContractId { get; init; }
    public List<AuthorDTO> Authors { get; init; } = new List<AuthorDTO>();
    public string Title { get; init; }
    public DateTime SignedDate { get; init; }
}
