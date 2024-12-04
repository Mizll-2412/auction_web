using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblWatchlist
{
    public int IWatchlistId { get; set; }

    public int? IUserId { get; set; }

    public int? IProductId { get; set; }

    public DateTime? DtAddedTime { get; set; }

    public virtual TblProduct? IProduct { get; set; }

    public virtual TblUser? IUser { get; set; }
}
