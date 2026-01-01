using BTL_LTWNC.Models;

namespace BTL_LTWNC.Repositories.Notification
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        public NotificationService(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }
        // thong bao co dau gia moi
        public async Task NotifyNewBid(int sellerId, int bidderId, int auctionId, int productId,
                                        string bidderName, decimal bidAmount, string productName)
        {
            var notification = new TblNotification
            {
                iUserId = sellerId,
                iSenderId = bidderId,
                iAuctionId = auctionId,
                iProductId = productId,
                sTitle = "Có người đấu giá sản phẩm của bạn",
                SMessage = $"{bidderName} đã đặt {bidAmount:N0} VND cho '{productName}'",
                SType = "bid",
                SUrl = $"/Auction/Details/{auctionId}",
                BIsRead = false
            };
            await _notificationRepo.CreateNotificationAsync(notification);
        }
        // thong bao khi co dau gia cao hon dau gia cua ban
        public async Task NotifyOutbid(int oldBidderId, int newBidderId, int auctionId, int productId,
                                        string newBidderName, decimal newBidAmount, string productName)
        {
            var notification = new TblNotification
            {
                iUserId = oldBidderId,
                iSenderId = newBidderId,
                iAuctionId = auctionId,
                iProductId = productId,
                sTitle = "Có người đấu giá cao hơn bạn",
                SMessage = $"{newBidderName} đã đặt giá {newBidAmount:N0} VND cho '{productName}', cao hơn giá của bạn",
                SType = "outbid",
                SUrl = $"/Auction/Details/{auctionId}",
                BIsRead = false
            };
            await _notificationRepo.CreateNotificationAsync(notification);
        }

        //Thong bao thang dau gia
        public async Task NotifyAuctionWin(int winnerId, int sellerId, int auctionId, int productId,
                                            string productName, decimal finalPrice)
        {
            var notification = new TblNotification
            {
                iUserId = winnerId,
                iSenderId = sellerId,
                iAuctionId = auctionId,
                iProductId = productId,
                sTitle = "Chúc mừng! Bạn đã thắng đấu giá",
                SMessage = $"Bạn đã thắng '{productName}' với giá {finalPrice:N0} VND. Vui lòng thanh toán.",
                SType = "auction_won",
                SUrl = $"/Transaction/Payment/{auctionId}",
                BIsRead = false
            };

            await _notificationRepo.CreateNotificationAsync(notification);
        }
        //thong bao sản phẩm đã bán
        public async Task NotifyProductSold(int sellerId, int winnerId, int auctionId, int productId,
                                            string winnerName, string productName, decimal finalPrice)
        {
            var notification = new TblNotification
            {
                iUserId = sellerId,
                iSenderId = winnerId,
                iAuctionId = auctionId,
                iProductId = productId,
                sTitle = "Sản phẩm của bạn đã được bán",
                SMessage = $"{winnerName} đã thắng đấu giá '{productName}' với giá {finalPrice:N0} VND",
                SType = "product_sold",
                SUrl = $"/Auction/Details/{auctionId}",
                BIsRead = false
            };

            await _notificationRepo.CreateNotificationAsync(notification);
        }
        //thong báo sắp kết thúc
        public async Task NotifyAuctionEnding(int userId, int auctionId, int productId,
                                              string productName, int minutesLeft)
        {
            var notification = new TblNotification
            {
                iUserId = userId,
                iAuctionId = auctionId,
                iProductId = productId,
                sTitle = "Phiên đấu giá sắp kết thúc",
                SMessage = $"Phiên đấu giá '{productName}' sẽ kết thúc trong {minutesLeft} phút",
                SType = "auction_ending",
                SUrl = $"/Auction/Details/{auctionId}",
                BIsRead = false
            };

            await _notificationRepo.CreateNotificationAsync(notification);
        }
    }
}