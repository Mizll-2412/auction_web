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

        public async Task<TblNotification> CreateNotificationAsync(TblNotification notification)
        {
            notification.DtCreatedTime = DateTime.Now;
            notification.BIsRead = false;
            _context.TblNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> CreateNotificationsAsync(List<TblNotification> notifications)
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

        public async Task<bool> DeleteAllNotificationsAsync(int userId)
        {
            try
            {
                var notifications = await _context.TblNotifications
                    .Where(n => n.iUserId == userId).ToListAsync();
                _context.TblNotifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var noti = await _context.TblNotifications.FindAsync(notificationId);
                if (noti != null)
                {
                    _context.TblNotifications.Remove(noti);
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

        public async Task<bool> DeleteOldNotificationsAsync(int days = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-days);
                var oldNotification = await _context.TblNotifications
                    .Where(n => n.DtCreatedTime < cutoffDate && n.BIsRead)
                    .ToListAsync();
                _context.TblNotifications.RemoveRange(oldNotification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TblNotification>> GetAllNotificationsByUserIdAsync(int userId)
        {
            return await _context.TblNotifications
                .Include(n => n.ISender)
                .Include(n => n.IAuction)
                .Include(n => n.IProduct)
                .Where(n => n.iUserId == userId)
                .OrderByDescending(n => n.DtCreatedTime)
                .ToListAsync();
        }

        public async Task<TblNotification?> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.TblNotifications
                .Include(n => n.ISender)
                .Include(n => n.IAuction)
                .Include(n => n.IProduct)
                .FirstOrDefaultAsync(n => n.iNotificationId == notificationId);
        }

        public async Task<List<TblNotification>> GetNotificationsByTypeAsync(int userId, string type)
        {
            return await _context.TblNotifications
                .Include(n => n.ISender)
                .Include(n => n.IAuction)
                .Include(n => n.IProduct)
                .Where(n => n.iUserId == userId && n.SType == type)
                .OrderByDescending(n => n.DtCreatedTime)
                .ToListAsync();
        }

        public async Task<List<TblNotification>> GetNotificationsByUserIdAsync(int userId, int take = 10)
        {
            return await _context.TblNotifications
                .Include(n => n.ISender)
                .Include(n => n.IAuction)
                .Include(n => n.IProduct)
                .Where(n => n.iUserId == userId)
                .OrderByDescending(n => n.DtCreatedTime)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.TblNotifications
                .CountAsync(n => n.iUserId == userId && !n.BIsRead);
        }

        public async Task<bool> HasNewNotificationsAsync(int userId, DateTime since)
        {
            return await _context.TblNotifications
                .AnyAsync(n => n.iUserId == userId 
                    && n.DtCreatedTime > since && !n.BIsRead);
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                var notifications = await _context.TblNotifications
                    .Where(n => n.iUserId == userId && !n.BIsRead)
                    .ToListAsync();
                foreach (var noti in notifications)
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

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var noti = await _context.TblNotifications.FindAsync(notificationId);
                if (noti != null)
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