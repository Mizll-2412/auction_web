using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblUsers")]
public partial class TblUser
{
    [Key]
    [Column("iUserId")]
    public int IUserId { get; set; }

    [Column("sPassword")]
    [Required]
    public string SPassword { get; set; } = null!;

    [Column("sEmail")]
    [Required]
    public string SEmail { get; set; } = null!;

    [Column("sFullName")]
    [Required]
    public string SFullName { get; set; } = null!;

    [Column("sPhoneNumber")]
    public string? SPhoneNumber { get; set; }

    [Column("sRole")]
    public string? SRole { get; set; }

    [Column("verifyKey")] // hoặc "VerifyKey" tùy tên cột trong DB của bạn
    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "VerifyKey phải có 6 ký tự.")]
    [RegularExpression(@"^\d{5}[0-9]$", ErrorMessage = "VerifyKey phải kết thúc bằng một chữ số.")]
    public string VerifyKey { get; set; }

    [InverseProperty(nameof(TblAuction.IWinner))]
    public virtual ICollection<TblAuction> TblAuctions { get; set; } = new List<TblAuction>();
    public virtual ICollection<TblBid> TblBids { get; set; } = new List<TblBid>();
    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
    public virtual ICollection<TblWatchlist> TblWatchlists { get; set; } = new List<TblWatchlist>();
    public virtual ICollection<TblNotification> TblNotificationsReceived { get; set; } = new List<TblNotification>();  
    public virtual ICollection<TblNotification> TblNotificationsSent  { get; set; } = new List<TblNotification>(); 

}