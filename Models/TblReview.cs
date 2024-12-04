using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblReview
{
    public int IReviewId { get; set; }

    public int? IProductId { get; set; }

    public int? IReviewerId { get; set; }

    public int? IRating { get; set; }

    public string? SComment { get; set; }

    public DateTime? DtReviewTime { get; set; }

    public virtual TblProduct? IProduct { get; set; }

    public virtual TblUser? IReviewer { get; set; }
}
