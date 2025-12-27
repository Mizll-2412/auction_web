using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblWatchlist")]
public partial class TblWatchlist
{
    [Key]
    [Column("iWatchlistId")]
    public int IWatchlistId { get; set; }

    [Column("iUserId")]
    public int? IUserId { get; set; }

    [Column("iProductId")]
    public int? IProductId { get; set; }

    [Column("dtAddedTime")]
    public DateTime? DtAddedTime { get; set; }

    [ForeignKey("IProductId")]
    public virtual TblProduct? IProduct { get; set; }

    [ForeignKey("IUserId")]
    public virtual TblUser? IUser { get; set; }
}