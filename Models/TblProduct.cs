using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

public partial class TblProduct
{
    public int IProductId { get; set; }

    public int? ISellerId { get; set; }

    public string SProductName { get; set; } = null!;

    public string? SDescription { get; set; }

    public decimal DStartingPrice { get; set; }

    public int? ICategoryId { get; set; }

    public string? SImageUrl { get; set; }

    public string? SStatus { get; set; }

    public virtual TblCategory? ICategory { get; set; }

    public virtual TblUser? ISeller { get; set; }

    public virtual ICollection<TblAuction> TblAuctions { get; set; } = new List<TblAuction>();

    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();

    public virtual ICollection<TblWatchlist> TblWatchlists { get; set; } = new List<TblWatchlist>();
}
