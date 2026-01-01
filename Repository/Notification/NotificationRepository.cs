using Microsoft.EntityFrameworkCore;
using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories.Notification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbBtlLtwncContext _context;
        public NotificationRepository(DbBtlLtwncContext context)
        {
            _context = context;
        }

        async Task<TblNotification> INotificationRepository.CreateNotificationAsync(TblNotification notification)
        {
            notification.DtCreatedTime = DateTime.Now;
            notification.BIsRead = false;
            _context.TblNotification.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        Task<bool> INotificationRepository.CreateNotificationsAsync(List<TblNotification> notifications)
        {
            try
            {
                foreach (var noti in notifications)
                {
                    noti.DtCreatedTime = DateTime.Now;
                    noti.BIsRead = false;
                }
                _context.TblNotifications.AddRange(notifications);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> INotificationRepository.DeleteAllNotificationsAsync(int userId)
        {
            try
            {
                var notifications = await _context.TblNotification
                                    .Where(n => n.IUserId == userId).ToListAsync();
                _context.TblNotification.Remove(notifications);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> INotificationRepository.DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var noti = await _context.TblNotification.FindAsync(notificationId);
                if (noti != null)
                {
                    _context.TblNotification.Remove(noti);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }catch
            {
                return false;
            }
        }

        async Task<bool> INotificationRepository.DeleteOldNotificationsAsync(int days=30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-days);
                var oldNotification = await _context.TblNotification
                .Where(n => n.DtCreatedTime < cutoffDate && n.BIsRead)
                .ToListAsync();
                _context.TblNotification.Remove(oldNotification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<List<TblNotification>> INotificationRepository.GetAllNotificationsByUserIdAsync(int userId)
        {
            return await _context.TblNotification
            .Include(n => n.ISender)
            .Include(n => n.IAuction)
            .Include(n => n.IProduct)
            .Where(n => n.IUserId == userId)
            .ToListAsync();
        }

        async Task<TblNotification?> INotificationRepository.GetNotificationByIdAsync(int notificationId)
        {
            return await _context.TblNotification
            .Include(n => n.ISender)
            .Include(n => n.IAuction)
            .Include(n => n.IProduct)
            .FirstOrDefaultAsync(n => n.iNotificationId == notificationId);
        }

        async Task<List<TblNotification>> INotificationRepository.GetNotificationsByTypeAsync(int userId, string type)
        {
            return await _context.TblNotification
            .Include(n => n.ISender)
            .Include(n => n.IAuction)
            .Include(n => n.IProduct)
            .Where(n => n.IUserId == userId && n.SType = type)
            .OrderByDescending(n => n.DtCreatedTime)
            .ToListAsync();
        }

        async Task<List<TblNotification>> INotificationRepository.GetNotificationsByUserIdAsync(int userId, int take)
        {
            return await _context.TblNotification
            .Include(n => n.ISender)
            .Include(n => n.IAuction)
            .Include(n => n.IProduct)
            .Where(n => n.IUserId == userId)
            .OrderByDescending(n => n.DtCreatedTime)
            .Take(take)
            .ToListAsync();
        }

        async Task<int> INotificationRepository.GetUnreadCountAsync(int userId)
        {
            return await _context.TblNotification.CountAsync(n => n.IUserId == userId && !n.BIsRead);
        }

        async Task<bool> INotificationRepository.HasNewNotificationsAsync(int userId, DateTime since)
        {
            return await _context.TblNotification
                        .AnyAsync(n => n.IUserId == userId 
                                && n.DtCreatedTime < since && !n.BIsRead);
        }

        async Task<bool> INotificationRepository.MarkAllAsReadAsync(int userId)
        {
            try
            {
                var notifications = await _context.TblNotification
                .Where(n => n.IUserId == userId && !n.BIsRead)
                .ToListAsync();
                foreach( var noti in notifications)
                {
                    noti.BIsRead = true;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> INotificationRepository.MarkAsReadAsync(int notificationId)
        {
            try
            {
                var noti = await _context.TblNotification.FindAsync(notificationId);
                if(noti != null)
                {
                    noti.BIsRead = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}