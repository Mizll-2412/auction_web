using Microsoft.EntityFrameworkCore;
using BTL_LTWNC.Models;
using BTL_LTWNC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Thời gian hết hạn session là 30 phút
    options.Cookie.HttpOnly = true;  // Đảm bảo cookie chỉ có thể truy cập qua HTTP
    options.Cookie.IsEssential = true;  // Cookie là thiết yếu
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Cookie sẽ chỉ được gửi qua HTTPS
});

builder.Services.AddDbContext<DbBtlLtwncContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// Cấu hình Authentication và Authorization
builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Định tuyến tới trang đăng nhập
        options.AccessDeniedPath = "/Account/AccessDenied";  // Định tuyến tới trang từ chối quyền truy cập
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("QuanTriVienOnly", policy => policy.RequireRole("Quản trị viên"));  // Thay đổi tên vai trò từ "Admin" thành "Quản trị viên"
});

var app = builder.Build();

// Middleware cấu hình cho môi trường sản xuất và phát triển
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Kích hoạt HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Đảm bảo các file tĩnh có thể truy cập

app.UseSession();  // Cấu hình Session

app.UseRouting();

// Kích hoạt Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến các controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
