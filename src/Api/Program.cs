using Argo.CA.Api;
using Argo.CA.Application;
using Argo.CA.Infrastructure;
using Argo.CA.Infrastructure.Identity;
using Argo.CA.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddApiServices();

var app = builder.Build();

app.MapGroup("/identity")
    .MapIdentityApi<ApplicationUser>()
    .WithTags("Identity");

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
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler(_ => { });

app.MapControllers();

app.Run();

namespace Argo.CA.Api
{
    public class Program;
}