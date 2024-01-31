using Infrastructure.Data.Services;
using PublisherMiniFrontEnd.Services;

namespace PublisherMiniFrontEnd;

public static class ContractingEndpoints
{
    public static void MapContractingEndpoints(this IEndpointRouteBuilder routes)
    {
        var groupContracts = routes.MapGroup("/api/contracts");
        var groupAuthors = routes.MapGroup("/api/authors");

        groupContracts.MapGet("/ByAuthorLastName{last}",
        async (string last, ContractSearchService searcher) =>
        await searcher.GetContractPickListForAuthorLastName(last));

        groupContracts.MapGet("/ByInitDateRange{start,end}",
            async (string start, string end, ContractSearchService searcher) =>
            await searcher.GetContractPickListForInitiatedDateRange
              (DateTime.Parse(start), DateTime.Parse(end)));
       
        groupAuthors.MapGet("/authors", async (ContractedAuthorsService authorService) =>
            await authorService.ListAllAuthorsAsync());

        groupAuthors.MapGet("/authors/firstname/{first}", async (string first, ContractedAuthorsService authorService) =>
            await authorService.GetAuthorsByFirstNameAsync(first));

        groupAuthors.MapGet("/authors/lastname/{last}", async (string last, ContractedAuthorsService authorService) =>
            await authorService.GetAuthorsByLastNameAsync(last));


    }
}