using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblTransaction
{
    public int ITransactionId { get; set; }

    public int? IAuctionId { get; set; }

    public int? IBuyerId { get; set; }

    public decimal DAmount { get; set; }

    public DateTime? DtTransactionTime { get; set; }

    public virtual TblAuction? IAuction { get; set; }

    public virtual TblUser? IBuyer { get; set; }
}
