using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblReviews")]
public partial class TblReview
{
    [Key]
    [Column("iReviewId")]
    public int IReviewId { get; set; }

    [Column("iProductId")]
    public int? IProductId { get; set; }

    [Column("iReviewerId")]
    public int? IReviewerId { get; set; }

    [Column("iRating")]
    public int? IRating { get; set; }

    [Column("sComment")]
    public string? SComment { get; set; }

    [Column("dtReviewTime")]
    public DateTime? DtReviewTime { get; set; }

    [ForeignKey("IProductId")]
    public virtual TblProduct? IProduct { get; set; }

    [ForeignKey("IReviewerId")]
    public virtual TblUser? IReviewer { get; set; }
}