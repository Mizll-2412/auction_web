using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTL_LTWNC.Models;

public partial class DbBtlLtwncContext : DbContext
{
    public DbBtlLtwncContext()
    {
    }

    public DbBtlLtwncContext(DbContextOptions<DbBtlLtwncContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAuction> TblAuctions { get; set; }

    public virtual DbSet<TblBid> TblBids { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblReview> TblReviews { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }
    public virtual DbSet<TblNotification> TblNotifications { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblWatchlist> TblWatchlists { get; set; }
    public object ProductAuctionViewModels { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAuction>(entity =>
        {
            entity.HasKey(e => e.IAuctionId).HasName("PK__tblAucti__0598248C4FC97FD0");

            entity.ToTable("tblAuctions");

            entity.Property(e => e.IAuctionId).HasColumnName("iAuctionId");
            entity.Property(e => e.DHighestBid)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dHighestBid");
            entity.Property(e => e.DtEndTime)
                .HasColumnType("datetime")
                .HasColumnName("dtEndTime");
            entity.Property(e => e.DtStartTime)
                .HasColumnType("datetime")
                .HasColumnName("dtStartTime");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IWinnerId).HasColumnName("iWinnerId");
            entity.Property(e => e.SStatus)
                .HasMaxLength(20)
                .HasColumnName("sStatus");

            entity.HasOne(d => d.IProduct).WithMany(p => p.TblAuctions)
                .HasForeignKey(d => d.IProductId)
                .HasConstraintName("FK__tblAuctio__iProd__52593CB8");

            entity.HasOne(d => d.IWinner).WithMany(p => p.TblAuctions)
                .HasForeignKey(d => d.IWinnerId)
                .HasConstraintName("FK__tblAuctio__iWinn__534D60F1");
        });

        modelBuilder.Entity<TblBid>(entity =>
        {
            entity.HasKey(e => e.IBidId).HasName("PK__tblBids__4DCCE6CCE7FAAD2D");

            entity.ToTable("tblBids");

            entity.Property(e => e.IBidId).HasColumnName("iBidId");
            entity.Property(e => e.DBidAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dBidAmount");
            entity.Property(e => e.DtBidTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dtBidTime");
            entity.Property(e => e.IAuctionId).HasColumnName("iAuctionId");
            entity.Property(e => e.IBidderId).HasColumnName("iBidderId");

            entity.HasOne(d => d.IAuction).WithMany(p => p.TblBids)
                .HasForeignKey(d => d.IAuctionId)
                .HasConstraintName("FK__tblBids__iAuctio__571DF1D5");

            entity.HasOne(d => d.IBidder).WithMany(p => p.TblBids)
                .HasForeignKey(d => d.IBidderId)
                .HasConstraintName("FK__tblBids__iBidder__5812160E");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.ICategoryId).HasName("PK__tblCateg__342A080C6C2378DE");

            entity.ToTable("tblCategories");

            entity.Property(e => e.ICategoryId).HasColumnName("iCategoryId");
            entity.Property(e => e.SCategoryName)
                .HasMaxLength(100)
                .HasColumnName("sCategoryName");
            entity.Property(e => e.SDescription)
                .HasMaxLength(255)
                .HasColumnName("sDescription");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.IProductId).HasName("PK__tblProdu__2A611C84142E7198");

            entity.ToTable("tblProducts");

            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.DStartingPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dStartingPrice");
            entity.Property(e => e.ICategoryId).HasColumnName("iCategoryId");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
            entity.Property(e => e.SDescription)
                .HasMaxLength(255)
                .HasColumnName("sDescription");
            entity.Property(e => e.SImageUrl)
                .HasMaxLength(255)
                .HasColumnName("sImageUrl");
            entity.Property(e => e.SProductName)
                .HasMaxLength(100)
                .HasColumnName("sProductName");
            entity.Property(e => e.SStatus)
                .HasMaxLength(20)
                .HasColumnName("sStatus");

            entity.HasOne(d => d.ICategory).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.ICategoryId)
                .HasConstraintName("FK__tblProduc__iCate__4E88ABD4");

            entity.HasOne(d => d.ISeller).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.ISellerId)
                .HasConstraintName("FK__tblProduc__iSell__4D94879B");
        });
        modelBuilder.Entity<TblNotification>(entity =>
        {
            entity.HasKey(e => e.iNotificationId);

            entity.ToTable("tblNotifications");

            entity.Property(e => e.iNotificationId).HasColumnName("iNotificationId");
            entity.Property(e => e.iUserId).HasColumnName("iUserId");
            entity.Property(e => e.iSenderId).HasColumnName("iSenderId");
            entity.Property(e => e.iAuctionId).HasColumnName("iAuctionId");
            entity.Property(e => e.iProductId).HasColumnName("iProductId");

            entity.Property(e => e.sTitle).HasColumnName("sTitle");
            entity.Property(e => e.SMessage).HasColumnName("sMessage");
            entity.Property(e => e.SType).HasColumnName("sType");
            entity.Property(e => e.SUrl).HasColumnName("sUrl");

            entity.Property(e => e.BIsRead)
                .HasDefaultValue(false)
                .HasColumnName("bIsRead");

            entity.Property(e => e.DtCreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dtCreatedTime");

            entity.HasOne(d => d.IUser)
                .WithMany(p => p.TblNotificationsReceived)
                .HasForeignKey(d => d.iUserId);

            entity.HasOne(d => d.ISender)
                .WithMany(p => p.TblNotificationsSent)
                .HasForeignKey(d => d.iSenderId);

            entity.HasOne(d => d.IAuction)
                .WithMany(p => p.TblNotifications)
                .HasForeignKey(d => d.iAuctionId);

            entity.HasOne(d => d.IProduct)
                .WithMany(p => p.TblNotifications)
                .HasForeignKey(d => d.iProductId);
        });


        modelBuilder.Entity<TblReview>(entity =>
        {
            entity.HasKey(e => e.IReviewId).HasName("PK__tblRevie__2339F988A2FCEC56");

            entity.ToTable("tblReviews");

            entity.Property(e => e.IReviewId).HasColumnName("iReviewId");
            entity.Property(e => e.DtReviewTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dtReviewTime");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IRating).HasColumnName("iRating");
            entity.Property(e => e.IReviewerId).HasColumnName("iReviewerId");
            entity.Property(e => e.SComment)
                .HasMaxLength(255)
                .HasColumnName("sComment");

            entity.HasOne(d => d.IProduct).WithMany(p => p.TblReviews)
                .HasForeignKey(d => d.IProductId)
                .HasConstraintName("FK__tblReview__iProd__60A75C0F");

            entity.HasOne(d => d.IReviewer).WithMany(p => p.TblReviews)
                .HasForeignKey(d => d.IReviewerId)
                .HasConstraintName("FK__tblReview__iRevi__619B8048");
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.ITransactionId).HasName("PK__tblTrans__66664CE92E08B295");

            entity.ToTable("tblTransactions");

            entity.Property(e => e.ITransactionId).HasColumnName("iTransactionId");
            entity.Property(e => e.DAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dAmount");
            entity.Property(e => e.DtTransactionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dtTransactionTime");
            entity.Property(e => e.IAuctionId).HasColumnName("iAuctionId");
            entity.Property(e => e.IBuyerId).HasColumnName("iBuyerId");

            entity.HasOne(d => d.Auction).WithMany(p => p.TblTransactions)
                .HasForeignKey(d => d.IAuctionId)
                .HasConstraintName("FK__tblTransa__iAuct__5BE2A6F2");

            entity.HasOne(d => d.Buyer).WithMany(p => p.TblTransactions)
                .HasForeignKey(d => d.IBuyerId)
                .HasConstraintName("FK__tblTransa__iBuye__5CD6CB2B");
        });
        

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.IUserId).HasName("PK__tblUsers__BA95FFB1DAF726A6");

            entity.ToTable("tblUsers");

            entity.Property(e => e.IUserId).HasColumnName("iUserId");
            entity.Property(e => e.SEmail)
                .HasMaxLength(100)
                .HasColumnName("sEmail");
            entity.Property(e => e.SFullName)
                .HasMaxLength(100)
                .HasColumnName("sFullName");
            entity.Property(e => e.SPassword)
                .HasMaxLength(100)
                .HasColumnName("sPassword");
            entity.Property(e => e.SPhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("sPhoneNumber");
        });
        modelBuilder.Entity<TblProduct>()
            .HasMany(p => p.TblWatchlists) // TblProduct có nhiều TblWatchlists
            .WithOne(w => w.IProduct) // Mỗi TblWatchlist liên kết với một TblProduct
            .HasForeignKey(w => w.IProductId) // Khóa ngoại là IProductId trong TblWatchlist
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TblProduct>()
        .HasMany(p => p.TblAuctions) // TblProduct có nhiều TblAuctions
        .WithOne(a => a.IProduct) // Mỗi TblAuction liên kết với một TblProduct
        .HasForeignKey(a => a.IProductId) // Khóa ngoại là IProductId trong TblAuction
        .OnDelete(DeleteBehavior.Cascade); // Xóa cascade nếu TblProduct bị xóa
        modelBuilder.Entity<TblProduct>()
                .HasMany(p => p.TblReviews) // TblProduct có nhiều TblReviews
                .WithOne(r => r.IProduct) // Mỗi TblReview liên kết với một TblProduct
                .HasForeignKey(r => r.IProductId) // Khóa ngoại là IProductId trong TblReview
                .OnDelete(DeleteBehavior.Cascade); // Xóa cascade nếu TblProduct bị xóa
        modelBuilder.Entity<TblWatchlist>(entity =>
        {
            entity.HasKey(e => e.IWatchlistId).HasName("PK__tblWatch__2DD480F39BCBC417");

            entity.ToTable("tblWatchlist");

            entity.Property(e => e.IWatchlistId).HasColumnName("iWatchlistId");
            entity.Property(e => e.DtAddedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dtAddedTime");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");

            entity.HasOne(d => d.IProduct).WithMany(p => p.TblWatchlists)
                .HasForeignKey(d => d.IProductId)
                .HasConstraintName("FK__tblWatchl__iProd__6754599E");

            entity.HasOne(d => d.IUser).WithMany(p => p.TblWatchlists)
                .HasForeignKey(d => d.IUserId)
                .HasConstraintName("FK__tblWatchl__iUser__66603565");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
