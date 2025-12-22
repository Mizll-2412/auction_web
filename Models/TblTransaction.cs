using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace BTL_LTWNC.Models;

public partial class TblTransaction
{
    public int ITransactionId { get; set; }

    public int? IAuctionId { get; set; }

    public int? IBuyerId { get; set; }

    public decimal DAmount { get; set; }

    public DateTime? DtTransactionTime { get; set; }

    [ForeignKey("IAuctionId")]
    public virtual TblAuction Auction { get; set; }
    
    [ForeignKey("IBuyerId")]
    public virtual TblUser Buyer { get; set; } 
}

