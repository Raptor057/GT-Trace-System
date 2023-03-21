using GT.Trace.EZ2000.Packaging.App.Services;

namespace GT.Trace.EZ2000.Packaging.Infra.Services
{
    public class NotificationsService : INotificationsService
    {
        public void Show(string message) => Console.WriteLine(message);
    }
}