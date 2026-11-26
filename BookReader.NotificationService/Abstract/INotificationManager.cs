using Microsoft.AspNetCore.Http;

namespace BookReader.NotificationService.Abstract
{
    public interface INotificationManager
    {
        void Add(int userId, HttpResponse response);
        void Remove(int userId, HttpResponse response);
        IEnumerable<HttpResponse> GetConnections(int userId);
        Task SendAsync<T>(int userId, T message);
    }
}
