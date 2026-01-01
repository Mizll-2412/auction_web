using Microsoft.AspNetCore.Mvc;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories.Notification;
using Newtonsoft.Json;
using BTL_LTWNC.Repositories;

namespace BTL_LTWNC.Controllers
{
    public class NotificationController: Controller
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationController(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }
        //get Notification/Index
        public async Task<IActionResult> Index()
        {
            var userJson = HttpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("Login", "Account");
            }
            var user = JsonConvert.DeserializeObject<TblUser>(userJson);
            var notifications = await _notificationRepo.GetAllNotificationsByUserIdAsync(user.IUserId);

            return View(notifications);
        }
        // get: Notification/GetNotifications
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var userJson = HttpContext.Session.GetString("UserSession");
                if (string.IsNullOrEmpty(userJson))
                {
                    return Json(new { success = false, message = "Chưa đăng nhập" });
                }
                var user = JsonConvert.DeserializeObject<TblUser>(userJson);
                var notifications = await _notificationRepo.GetNotificationsByUserIdAsync(user.IUserId);
                var unreadCount= await _notificationRepo.GetUnreadCountAsync(user.IUserId);
                var result = notifications.Select( n => new
                {
                    id = n.iNotificationId,
                    title = n.sTitle,
                    message = n.SMessage,
                    url = n.SUrl,
                    isRead = n.BIsRead,
                    timeAgo = GetTimeAgo(n.DtCreatedTime),
                    senderName = n.ISender?.SFullName,
                    productName = n.IProduct?.SProductName
                }).ToList();
                return Json(new
                {
                   success = true,
                   notifications = result,
                   unreadCount = unreadCount 
                });
            }
            catch (Exception ex)
            {
               return Json(new {success = false, message = ex.Message}) ;
            }
        }
        private string GetTimeAgo(DateTime? dateTime)
        {
            if (dateTime == null) return "";

            var timeSpan = DateTime.Now - dateTime.Value;

            if (timeSpan.TotalMinutes < 1)
                return "Vừa xong";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            
            return dateTime.Value.ToString("dd/MM/yyyy");
        }

    }
}