using System.ComponentModel.DataAnnotations;

namespace BTL_LTWNC.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string SEmail { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string SPassword { get; set; }
    }
}
