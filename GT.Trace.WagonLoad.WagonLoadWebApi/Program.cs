using GT.Trace.Common.CleanArch;
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
//builder.Services.AddAppServices();
//builder.Services.AddInfraServices(configuration);
builder.Services.AddSingleton(typeof(ResultViewModel<>));
builder.Services.AddSingleton(configuration);
builder.Services.AddSignalR();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services
    .AddMvc(o =>
    {
        o.Filters.Add(new Microsoft.AspNetCore.Mvc.ResponseCacheAttribute { NoStore = true, Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.None });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();
app.UseCors();
//app.MapHub<EtiMovementsHub>("/hubs/etimovements").RequireCors(cors);

app.MapControllers();

app.Run();