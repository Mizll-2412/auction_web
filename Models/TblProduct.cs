using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblProducts")]
public partial class TblProduct
{
    [Key]
    [Column("iProductId")]
    public int IProductId { get; set; }

    [Column("sProductName")]
    public string? SProductName { get; set; }

    [Column("sDescription")]
    public string? SDescription { get; set; }

    [Column("dStartingPrice")]
    public decimal? DStartingPrice { get; set; }

    [Column("sImageUrl")]
    public string? SImageUrl { get; set; }

    [Column("iCategoryId")]
    public int? ICategoryId { get; set; }

    [Column("iSellerId")]
    public int? ISellerId { get; set; }

    [Column("sStatus")]
    public string? SStatus { get; set; }

    [ForeignKey("ICategoryId")]
    public virtual TblCategory? ICategory { get; set; }

    [ForeignKey("ISellerId")]
    public virtual TblUser? ISeller { get; set; }

    public virtual ICollection<TblAuction> TblAuctions { get; set; } = new List<TblAuction>();

    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();
    public virtual ICollection<TblWatchlist> TblWatchlists { get; set; } = new List<TblWatchlist>();
    public virtual ICollection<TblNotification> TblNotifications { get; set; } = new List<TblNotification>();


}
  