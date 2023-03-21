/*
Este código es una aplicación web básica creada con ASP.NET Core. Primero, se importan los namespaces necesarios, incluyendo el paquete Serilog, que se utiliza para la creación de logs.
Luego, se define una política CORS que permite cualquier origen, método y encabezado y establece la propiedad AllowCredentials como verdadera.
Después, se crea una instancia del constructor WebApplication, que proporciona un punto de partida para configurar la aplicación web y agregar servicios al contenedor de servicios. En este caso, se agregan los servicios de CORS y controladores.
A continuación, se carga la configuración de la aplicación desde el archivo appsettings.json. La sección "CustomLogging" se usa para configurar el logger de Serilog, que se agrega como un servicio de logging.
Finalmente, se construye y se ejecuta la aplicación, que usa la política CORS creada anteriormente, mapea las rutas de los controladores y ejecuta la aplicación web.
 */
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Serilog.Events;

Action<CorsPolicyBuilder> cors = builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
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
app.UseCors();
app.MapControllers();
app.Run();
