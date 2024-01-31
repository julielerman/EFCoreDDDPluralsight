using PublisherSystem.SharedKernel.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel;

namespace PublisherSystem.SharedKernel.DTOs
{
    public class AuthorDTO
    {
      
        public AuthorDTO(string firstName, string lastName, string email, string phone, bool signed, Guid signedId)
        {
            Name = new PersonName(firstName, lastName);
            if (signed)
            { SignedAuthorId = signedId; }
            Signed = signed;
            Phone = phone;
            Email = email;
        }

        public bool Signed { get; init; }
        public Guid SignedAuthorId { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public PersonName Name { get; init; }
     

    }
}