using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App;
using GT.Trace.Etis.Infra;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;

Action<CorsPolicyBuilder> cors = builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddDefaultPolicy(cors));
builder.Services.AddControllers();
builder.Services.AddAppServices();
builder.Services.AddInfraServices(configuration);
builder.Services.AddSingleton(typeof(ResultViewModel<>));
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSingleton<IConfigurationRoot>(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();