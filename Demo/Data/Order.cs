using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }
    public string? CachThanhToan {  get; set; }
    public int? TransactStatusId { get; set; }

    public bool? Deleted { get; set; }

    public bool? Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int? PaymentId { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
