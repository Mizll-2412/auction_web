using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_LTWNC.Models;

[Table("tblCategories")]
public partial class TblCategory
{
    [Key]
    [Column("iCategoryId")]
    public int ICategoryId { get; set; }

    [Column("sCategoryName")]
    public string SCategoryName { get; set; } = null!;

    [Column("sDescription")]
    public string? SDescription { get; set; }

    [Column("imageUrl")]
    public string? ImageUrl { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}