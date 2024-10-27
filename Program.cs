using Smokeball_SEORank_Api.Models;
using Smokeball_SEORank_Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Config/config.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<SearchEngineSettings>(builder.Configuration.GetSection("SearchEngine"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ISeoRankService, SeoRankService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policyBuilder.WithOrigins("http://localhost:3000")
                         .AllowAnyHeader()
                         .AllowAnyMethod();
        }
        else
        {
            policyBuilder.WithOrigins("https://smokeball-serorank-web.azurewebsites.net")
                         .AllowAnyHeader()
                         .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapPost("/seo-rankings", async (HttpContext httpContext,ISeoRankService searchService, SearchRequest request) =>
{
    try{
        var positions = await searchService.PerformSeoRankSearch(request.Keywords, request.Url);
        return Results.Ok(positions);
    }
    catch(HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
    {
        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.TooManyRequests;
        return Results.Json(new ErrorResponse { ErrorCode = "TOO_MANY_REQUESTS" });
    }
    catch(Exception ex)
    {
        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        return Results.Json(new ErrorResponse { ErrorCode = "INTERNAL_SERVER_ERROR" });
    }
});

app.Run();
