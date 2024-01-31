using Microsoft.EntityFrameworkCore;
using PublisherSystem.SharedKernel.DTOs;

namespace Infrastructure.Data.Services;

public class ContractSearchService
{
    SearchContext _context;

    public ContractSearchService(SearchContext context)
    {
        _context = context;
    }

    public async Task<List<GuidKeyAndDescription>> 
        GetContractPickListForAuthorLastName(string lastnameStart)
    {
        return await _context.SearchResults.FromSql
            ($"GetContractsForAuthorLastNameStartswith {lastnameStart}").ToListAsync();
    }

    public async Task<List<GuidKeyAndDescription>> 
        GetContractPickListForInitiatedDateRange(DateTime datestart, DateTime dateend)
    {
        return await _context.SearchResults.FromSql
            ($"GetContractsInitiatedInDateRange {datestart},{dateend}").ToListAsync();


    }
    //other options 
    //all contracts? Unsigned contracts? Abandoned contracts?
}
