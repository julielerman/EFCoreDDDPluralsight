﻿using PublisherSystem.SharedKernel;
//THIS CLASS IS STAKE IN THE GROUND. More indepth in Module 8 (Many-to-Many)


namespace AuthorAndBookMaintenance.DomainModels;

public class Author : BaseEntity<int>
{
    public string FullName { get; set; }
    public string PreferredName { get; set; }
    public string Pronouns { get; set; }
    public List<string> Nicknames { get; set; }
    public string EmailAddress { get; set; }
    public string? Bio { get; set; }
    public IList<Book> Books { get; private set; } = new List<Book>();
 
    public Author(string fullName,
      string preferredName,
      string salutation,
      string emailAddress)
    {
        FullName = fullName;
        PreferredName = preferredName;
        Pronouns = salutation;
        EmailAddress = emailAddress;
    }

    public override string ToString()
    {
        return FullName.ToString();
    }
}
