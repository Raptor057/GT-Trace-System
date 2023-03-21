/*
Este c�digo es una aplicaci�n web b�sica creada con ASP.NET Core. Primero, se importan los namespaces necesarios, incluyendo el paquete Serilog, que se utiliza para la creaci�n de logs.
Luego, se define una pol�tica CORS que permite cualquier origen, m�todo y encabezado y establece la propiedad AllowCredentials como verdadera.
Despu�s, se crea una instancia del constructor WebApplication, que proporciona un punto de partida para configurar la aplicaci�n web y agregar servicios al contenedor de servicios. En este caso, se agregan los servicios de CORS y controladores.
A continuaci�n, se carga la configuraci�n de la aplicaci�n desde el archivo appsettings.json. La secci�n "CustomLogging" se usa para configurar el logger de Serilog, que se agrega como un servicio de logging.
Finalmente, se construye y se ejecuta la aplicaci�n, que usa la pol�tica CORS creada anteriormente, mapea las rutas de los controladores y ejecuta la aplicaci�n web.
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
