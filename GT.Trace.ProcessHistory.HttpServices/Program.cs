using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Serilog.Events;

Action<CorsPolicyBuilder> cors = builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => 
                true)
                .AllowCredentials();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddDefaultPolicy(cors));
builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();

builder.Services.AddLogging(logging =>
{
    var section = configuration.GetSection("CustomLogging");
    var serilogLogger = new LoggerConfiguration()
        .Enrich.WithProperty("Project", section.GetSection("Project").Value)
        .WriteTo.Seq(
            serverUrl: section.GetSection("SeqUri").Value,
            restrictedToMinimumLevel: (LogEventLevel)Enum.Parse(typeof(LogEventLevel), section.GetSection("LogEventLevel").Value))
        .CreateLogger();
    logging.AddSerilog(logger: serilogLogger, dispose: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();