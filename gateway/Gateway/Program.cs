using Gateway.Helpers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", true);

var securityKey = builder.Configuration.GetSection("Jwt")["Key"];

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot().AddKubernetes();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

// Add Authorization
var config = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware
        = async (downStreamContext, next) =>
        await JwtMiddleware.CreateAuthorizationFilter(downStreamContext, securityKey, next)
};

app.UseOcelot(config).Wait();

app.Run();