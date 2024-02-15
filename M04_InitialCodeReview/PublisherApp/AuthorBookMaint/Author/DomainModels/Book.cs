using PublisherSystem.SharedKernel;
using System.ComponentModel;
//THIS CLASS IS STAKE IN THE GROUND. More indepth in Module 8 (Many-to-Many)
namespace AuthorAndBookMaintenance.DomainModels;

        public class Book : BaseEntity<int>
{
    public Book(int authorId, string title, Category primaryCategory)
    {
        AuthorId = authorId;
        Title = title;
        PrimaryCategory = primaryCategory;
        Categories = new List<Category> { primaryCategory };
    }

    public int AuthorId { get; set; }
    public string Title { get; set; }
    public DateOnly PublishDate { get; set; }
    public List<DateOnly> EditionDates { get; set; } = new ();
    public string? ISBN { get; set; }
    public decimal USListPrice { get; set; }
    public Category PrimaryCategory { get; set; }
    public List<Category> Categories { get; set; } = new();

    public override string ToString()
    {
        return Title;
    }
}



