using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Demo.ModelViews
{
    public class RegisterViewModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string CustomerId { get; set; }

        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        [Display(Name = "Email")]

        public string Email { get; set; }

        [MaxLength(10, ErrorMessage = "Tối đa 10 kí tự")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Điện thoại")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage ="Chưa đúng định dạng di động Việt Nam")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }

        [MaxLength(250)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Địa chỉ khách hàng")]
        public string Address { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? Birthday { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 kí tự")]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Mật Khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần nhập mật khẩu tối thiểu 5 kí tự")]
        [Display(Name = "Nhập lại mật Khẩu")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
