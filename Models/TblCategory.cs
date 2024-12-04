using System;
using System.Collections.Generic;

namespace BTL_LTWNC.Models;

public partial class TblCategory
{
    public int ICategoryId { get; set; }

    public string SCategoryName { get; set; } = null!;

    public string? SDescription { get; set; }

    public string ImageUrl {get; set;}

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}
