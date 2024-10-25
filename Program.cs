using Smokeball_SEORank_Api.Models;
using Smokeball_SEORank_Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISeoRankService, SeoRankService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapPost("/seo-rankings", async (ISeoRankService searchService, SearchRequest request) =>
{
    var positions = await searchService.PerformSeoRankSearch(request.Keywords, request.Url);
    return Results.Ok(positions);
});

app.Run();
