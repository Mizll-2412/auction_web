using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblBids")]
public partial class TblBid
{
    [Key]
    [Column("iBidId")]
    public int IBidId { get; set; }

    [Column("iAuctionId")]
    public int? IAuctionId { get; set; }

    [Column("iBidderId")]
    public int? IBidderId { get; set; }

    [Column("dBidAmount")]
    public decimal? DBidAmount { get; set; }

    [Column("dtBidTime")]
    public DateTime? DtBidTime { get; set; }

    [ForeignKey("IAuctionId")]
    public virtual TblAuction? IAuction { get; set; }

    [ForeignKey("IBidderId")]
    public virtual TblUser? IBidder { get; set; }
}