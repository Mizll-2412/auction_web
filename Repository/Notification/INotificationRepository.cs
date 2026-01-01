using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories
{
    public interface INotificationRepository
    {
        Task<List<TblNotification>> GetNotificationsByUserIdAsync(int userId, int take = 10);
        Task<List<TblNotification>> GetAllNotificationsByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task<TblNotification?> GetNotificationByIdAsync(int notificationId);
        Task<TblNotification> CreateNotificationAsync(TblNotification notification);
        Task<bool> CreateNotificationsAsync(List<TblNotification> notifications);
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAllAsReadAsync(int userId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<bool> DeleteAllNotificationsAsync(int userId);
        Task<bool> DeleteOldNotificationsAsync(int days = 30);
        Task<List<TblNotification>> GetNotificationsByTypeAsync(int userId, string type);
        Task<bool> HasNewNotificationsAsync(int userId, DateTime since);
    }
}