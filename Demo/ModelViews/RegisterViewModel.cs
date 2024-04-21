using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Demo.ModelViews
{
    public class RegisterViewModel
    {
        public int CustomerId { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts ")]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        
        public string Email { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Display(Name = "Điện thoại")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage ="Chưa đúng định dạng di động Việt Nam")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh của bạn")]
        [Display(Name = "Ngày sinh")]
        public DateTime? Date { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 kí tự")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 kí tự")]
        [Display(Name = "Nhập lại mật Khẩu")]
        [Compare("Password",ErrorMessage ="Vui lòng nhập mật khẩu giống nhau" )]
        public string ConfirmPassword { get; set; }
    }
}
