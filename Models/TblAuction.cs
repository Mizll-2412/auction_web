using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblAuction
{
    public int IAuctionId { get; set; }

    public int? IProductId { get; set; }

    public DateTime DtStartTime { get; set; }

    public DateTime DtEndTime { get; set; }

    public decimal? DHighestBid { get; set; }

    public int? IWinnerId { get; set; }

    public string? SStatus { get; set; }

    public virtual TblProduct? IProduct { get; set; }

    public virtual TblUser? IWinner { get; set; }

    public virtual ICollection<TblBid> TblBids { get; set; } = new List<TblBid>();

    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();
}
