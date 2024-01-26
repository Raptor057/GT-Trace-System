using GT.Trace.BomSnapShotWebApi;
using Microsoft.AspNetCore.Cors.Infrastructure;

Action<CorsPolicyBuilder> cors = builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddDefaultPolicy(cors));
builder.Services.AddControllers();
builder.Services.AddSnapshotServices();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();