using System.ComponentModel.DataAnnotations;

namespace BTL_LTWNC.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string SEmail { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string SFullName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string SPassword { get; set; }

        [Compare("SPassword", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string SPhoneNumber { get; set; }
    }
}
