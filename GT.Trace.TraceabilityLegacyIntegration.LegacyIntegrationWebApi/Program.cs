using GT.Trace.TraceabilityLegacyIntegration.LegacyIntegrationWebApi.Hubs;
using GT.Trace.TraceabilityLegacyIntegration.LegacyIntegrationWebApi;
using Microsoft.AspNetCore.Cors.Infrastructure;

Action<CorsPolicyBuilder> cors = builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddCors(options => options.AddDefaultPolicy(cors));
builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddSignalR();
var app = builder.Build();

//Configure the HTTP request pipeline.


app.UseCors();
app.MapHub<EventsHub>("/hubs/events").RequireCors(cors);
app.MapControllers();
app.Run();