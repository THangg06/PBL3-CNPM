using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class RegisterViewModel
   {
        [Required(ErrorMessage ="Ban can nhap ten")]
        //public int CustomerID { get; set; }

        public string FullName { get; set; }
        [Required(ErrorMessage = "Ban can dia chi")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Ban can email")]
        [EmailAddress(ErrorMessage ="Dia chi email khong dung")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ban can nhap mat khau")]
        [MinLength(5,ErrorMessage ="Mat khau phai co it nhat 5 ki tu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Ban can nhap sdt")]
        public string Phone { get; set; }
  
          
    }
}
