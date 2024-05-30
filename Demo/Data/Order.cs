using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Data;

public partial class Order
{
    [Key]
    public int OrderID { get; set; }

    public string? CustomerId { get; set; } // Thêm dấu ? để nullable

    public string? FullName { get; set; } // Thêm dấu ? để nullable

    public string? Phone { get; set; } // Thêm dấu ? để nullable

    public string? Address { get; set; } = null;

    public DateTime? OrderDate { get; set; } // Chuyển thành nullable

    public DateTime? ShipDate { get; set; }

    public string? CachThanhToan { get; set; } = null;
    public decimal? TongTien { get; set; }
    public int? TransactStatusId { get; set; }

    public bool? Deleted { get; set; }

    public bool? Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentId { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
