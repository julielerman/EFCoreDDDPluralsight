
using AuthorBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthorBook.API;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/authors");
        group.MapGet("/", async (AuthorBookContext context) =>
            await context.Authors.Select(a => new { Name = a.FullName(), a.Id }).ToListAsync());

        group.MapGet("/firstandlastname/{first}/{last}", async (string first, string last, AuthorBookContext context) =>
            await context.Authors.Where(a => a.Name.FirstName.ToLower() == first.ToLower() &&
               a.Name.LastName.ToLower() == last.ToLower())
            .Select(a => new { Name = a.FullName(), a.Id }).ToListAsync());

        group.MapGet("/firstname/{first}", async (string first, AuthorBookContext context) =>
            await context.Authors
            .Where(a =>a.Name.FirstName.ToLower().StartsWith(first.ToLower()))
            .Select(a => new { Name = a.FullName(), a.Id }).ToListAsync());

        group.MapGet("/lastname/{last}", async (string last, AuthorBookContext context) =>
            await context.Authors
            .Where(a => a.Name.LastName.ToLower().StartsWith(last.ToLower()))
            .Select(a => new { Name = a.FullName(), a.Id }).ToListAsync());

        group.MapPost("/seed", async (AuthorBookContext context) =>
             await Seeder.SeedTheData(context));

    }
}