using AuthorAndBookMaintenance.DomainModels;
using AuthorBook.API;
using AuthorBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublisherSystem.SharedKernel.DTOs;

namespace IntegrationTests
{
    [TestClass]
    public class ServiceTests
    {
        AuthorBookContext _context;
        public ServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthorBookContext>();
            optionsBuilder.UseSqlServer(
                 "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=AuthorBookIntegrationTests");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
            optionsBuilder.AddInterceptors(new MyMaterializationInterceptor());
            var _options = optionsBuilder.Options;
            _context = new AuthorBookContext(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [TestMethod]
        public void CanCreateNewAuthorFromContractMessage()
        {
            var authorDto = new AuthorDTO("name", "last", "email", "phone", true, Guid.NewGuid());
            var author = Author.CreateFromNewContractMessage(authorDto.Name, authorDto.Email, Guid.NewGuid());
            CollectionAssert.AreEqual(new object[] { authorDto.Name, authorDto.Email }, new object[] { author.Name, author.EmailAddress });
        }

        [TestMethod]
        public void CanAddAuthorsAndBookForNewContract()
        {
            var authorDto = new AuthorDTO("name", "last", "email", "phone", false, Guid.NewGuid());
            var incomingContractMessage = new ContractSignedEventMessage
            {
                Authors = new List<AuthorDTO> { authorDto },
                ContractId = Guid.NewGuid(),
                SignedDate = DateTime.Now,
                Title = "title"
            };
           
                var service = new NewContractService(_context);
                service.AddAuthorsAndBookForNewContract(incomingContractMessage);
                _context.ChangeTracker.Clear();
                var author = _context.Authors.Include(a=>a.Books).First();
                Assert.AreEqual(incomingContractMessage.ContractId, author.ContractIds.First());
                Assert.AreEqual(incomingContractMessage.Title, author.Books.First().Title);
            }
        }
    }
