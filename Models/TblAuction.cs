using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblAuctions")]
public partial class TblAuction
{
    [Key]
    [Column("iAuctionId")]
    public int IAuctionId { get; set; }

    [Column("iProductId")]
    public int? IProductId { get; set; }

    [Column("dtStartTime")]
    public DateTime? DtStartTime { get; set; }

    [Column("dtEndTime")]
    public DateTime? DtEndTime { get; set; }

    [Column("dHighestBid")]
    public decimal? DHighestBid { get; set; }

    [Column("iWinnerId")]
    public int? IWinnerId { get; set; }

    [Column("sStatus")]
    public string? SStatus { get; set; }

    [ForeignKey("IProductId")]
    public virtual TblProduct? IProduct { get; set; }

    [ForeignKey("IWinnerId")]
    public virtual TblUser? IWinner { get; set; }

    public virtual ICollection<TblBid> TblBids { get; set; } = new List<TblBid>();

    public virtual ICollection<TblWatchlist> TblWatchlists { get; set; } = new List<TblWatchlist>();
    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
    public virtual ICollection<TblNotification> TblNotifications { get; set; } = new List<TblNotification>();


}