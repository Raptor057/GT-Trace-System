using GT.Trace.Packaging.App.Services;

namespace GT.Trace.Packaging.Infra.Services
{
    public class NotificationsService : INotificationsService
    {
        public void Show(string message) => Console.WriteLine(message);
    }
}