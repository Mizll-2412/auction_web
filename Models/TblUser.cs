using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BTL_LTWNC.Models;

public partial class TblUser
{
    public int IUserId { get; set; }

    public string SPassword { get; set; } = null!;

    public string SEmail { get; set; } = null!;

    public string SFullName { get; set; } = null!;

    public string? SPhoneNumber { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "VerifyKey phải có 6 ký tự.")]
    [RegularExpression(@"^\d{5}[0-9]$", ErrorMessage = "VerifyKey phải kết thúc bằng một chữ số.")]
    public string VerifyKey { get; set; }    
    public string? SRole {get;set;}
    public virtual ICollection<TblAuction> TblAuctions { get; set; } = new List<TblAuction>();

    public virtual ICollection<TblBid> TblBids { get; set; } = new List<TblBid>();

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();

    public virtual ICollection<TblReview> TblReviews { get; set; } = new List<TblReview>();

    public virtual ICollection<TblTransaction> TblTransactions { get; set; } = new List<TblTransaction>();

    public virtual ICollection<TblWatchlist> TblWatchlists { get; set; } = new List<TblWatchlist>();
}
