using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblBid
{
    public int IBidId { get; set; }

    public int? IAuctionId { get; set; }

    public int? IBidderId { get; set; }

    public decimal DBidAmount { get; set; }

    public DateTime? DtBidTime { get; set; }

    public virtual TblAuction? IAuction { get; set; }

    public virtual TblUser? IBidder { get; set; }
}
