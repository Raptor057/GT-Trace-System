// See https://aka.ms/new-console-template for more information
using System.Net.Mail;

var response = Execute(Environment.GetCommandLineArgs());

Log(response.Message);

Utils.Response Execute(string[] commandLineArgs)
{
    Utils.Response response;
    if (commandLineArgs.Length > 0 && !string.IsNullOrWhiteSpace(commandLineArgs[0]))
    {
        var targetDirectory = commandLineArgs[0];
        if (Directory.Exists(targetDirectory))
        {
            if (DirectoryIsEmpty(targetDirectory))
            {
                response = Utils.FailureResponse.DirectoryIsEmpty(targetDirectory);
                SendEmailNotification();
            }
            else
            {
                response = new Utils.SuccessResponse();
            }
        }
        else
        {
            response = Utils.FailureResponse.DirectoryDoesNotExists(targetDirectory);
        }
    }
    else
    {
        response = Utils.FailureResponse.MissingCommandLineArgument();
    }
    return response;
}

void Log(string message) => Console.WriteLine(message);

void SendEmailNotification() => new SmtpClient("host", 0).SendAsync("from", "to", "subject", "message", null);

bool DirectoryIsEmpty(string directoryPath) => Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories).Length == 0;

namespace Utils
{
    abstract record Response(string Message);

    sealed record SuccessResponse() : Response("OK");

    sealed record FailureResponse(string Message) : Response(Message)
    {
        public static FailureResponse MissingCommandLineArgument() => new FailureResponse("El directorio de busqueda no fue especificado en los argumentos de la linea de comandos.");

        public static FailureResponse DirectoryDoesNotExists(string Path) => new FailureResponse($"El directorio [ {Path} ] no existe.");

        public static FailureResponse DirectoryIsEmpty(string Path) => new FailureResponse($"El directorio [ {Path} ] se encuentra vacio.");
    }
}