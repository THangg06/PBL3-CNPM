

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Data
{
  
    public class ReviewProduct
    {
       
        public int Id { get; set; }
        public string? ProductId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int Rate { get; set; }
        public string? Content { get; set; }
        public string? Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }
        
    }
}
