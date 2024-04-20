using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Demo.ModelViews
{
    public class LoginViewModel
    {
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts ")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string Email { get; set; }

        [MinLength(5, ErrorMessage ="Bạn cần nhập mật khẩu tối thiểu 5 kí tự")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

    }
}
