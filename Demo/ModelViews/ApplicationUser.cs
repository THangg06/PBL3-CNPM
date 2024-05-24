using Demo.Data;
using System;
using System.Collections.Generic;

namespace Demo.ModelViews
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }

        // Các thuộc tính bổ sung
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }

        // Quan hệ với các thực thể khác
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
