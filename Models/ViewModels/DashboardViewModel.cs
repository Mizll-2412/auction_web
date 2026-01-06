using BTL_LTWNC.Models;

namespace BTL_LTWNC.ViewModels
{
    public class DashboardViewModel
    {
        // Stats
        public int TotalUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int TotalPosts { get; set; }
        public int NewPostsThisMonth { get; set; }
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }

        public List<string> UserChartLabels { get; set; }
        public List<int> UserChartData { get; set; }
        public List<int> PostStatusData { get; set; } // [Ongoing, Upcoming, Future, Ended]

        public List<TblUser> RecentUsers { get; set; }
        public List<PostViewModel> PendingPosts { get; set; }

        public DashboardViewModel()
        {
            UserChartLabels = new List<string>();
            UserChartData = new List<int>();
            PostStatusData = new List<int> { 0, 0, 0, 0 };
            RecentUsers = new List<TblUser>();
            PendingPosts = new List<PostViewModel>();
        }
    }

    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Views { get; set; }
    }
}