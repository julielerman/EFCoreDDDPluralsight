using Infrastructure.Data;
using Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using PublisherMiniFrontEnd;
using PublisherMiniFrontEnd.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped
    (sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["baseUrls:apiBase"]) });
builder.Services.AddScoped<ContractedAuthorsService>();
builder.Services.AddScoped<ContractSearchService>();
builder.Services.AddDbContext<SearchContext>
    (opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PubDB")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapContractingEndpoints();
app.Run();

