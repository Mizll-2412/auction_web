using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("TblNotification")]
public partial class TblNotification
{
    [Key]
    [Column("iNotificationId")]
    public int iNotificationId { get; set; }
    [Column("iUserId")]
    public int? iUserId { get; set; }
    [Column("iSenderId")]
    public int? iSenderId { get; set; }
    [Column("iAuctionId")]
    public int iAuctionId { get; set; }
    [Column("iProductId")]
    public int iProductId { get; set; }
    [Column("sTitle")]
    [MaxLength(255)]
    public string? sTitle { get; set; }
    [Column("sMessage")]
    public string? SMessage { get; set; }
    [Column("sType")]
    [MaxLength(50)]
    public string? SType { get; set; }
    [Column("sUrl")]
    [MaxLength(500)]
    public string? SUrl { get; set; }

    [Column("bIsRead")]
    public bool BIsRead { get; set; } = false;

    [Column("dtCreatedTime")]
    public DateTime? DtCreatedTime { get; set; }
    [ForeignKey("IUserId")]
    public virtual TblUser? IUser { get; set; } 

    [ForeignKey("ISenderId")]
    public virtual TblUser? ISender { get; set; } 

    [ForeignKey("IAuctionId")]
    public virtual TblAuction? IAuction { get; set; }  

    [ForeignKey("IProductId")]
    public virtual TblProduct? IProduct { get; set; } 
}