using GT.Trace.Changeover.App;
using GT.Trace.Changeover.Infra;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Action<CorsPolicyBuilder> cors = builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials();

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

// Add services to the container.
builder.Services.AddCors(options => options.AddDefaultPolicy(cors));
builder.Services.AddControllers();
builder.Services.AddAppServices();
builder.Services.AddInfraServices(configuration);
builder.Services.AddSingleton(typeof(GenericViewModel<>));
builder.Services.AddSingleton(configuration);
//builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();