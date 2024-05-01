using Argo.CA.Api;
using Argo.CA.Api.Infrastructure.ExceptionHandling;
using Argo.CA.Application;
using Argo.CA.Infrastructure;
using Argo.CA.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddApiServices();

var app = builder.Build();

// TODO: in what environments should the DB be migrated
if (app.Environment.IsDevelopment())
{
    await app.Services.InitialiseDatabaseAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCustomExceptionHandlers();

app.MapControllers();

app.Run();

namespace Argo.CA.Api
{
    public class Program;
}