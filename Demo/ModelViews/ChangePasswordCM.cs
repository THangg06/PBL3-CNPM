using System.ComponentModel.DataAnnotations;

namespace Demo.ModelViews
{
    public class ChangePasswordCM
    {
        [Key]
        //  [Required]
        //  public string CustomerID {  get; set; }
        [Display(Name = "Mat khau hien tai")]
        public string CurrentPassword { get; set; }
        [Display(Name = "Mat khau moi")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(5, ErrorMessage = "Ban can dat mat khau toi thieu 5 ki tu")]
        public string NewPassword { get; set; }
        [MinLength(5, ErrorMessage = "Ban can dat mat khau toi thieu 5 ki tu")]
        [Display(Name = "Nhap lai mat khau moi")]
        //  [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }
}