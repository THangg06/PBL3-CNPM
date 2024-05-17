using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Data;

public partial class OrderDetail
{
    //   [Key]
    public int OrderDetailId { get; set; }

    public int OrderID { get; set; }

    public string? ProductId { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public decimal? Total { get; set; }

    public DateTime? ShipDate { get; set; }

    public virtual Order? Order { get; set; }
    //  public virtual Order OderID { get; set; } = null;
    public virtual Product? Product { get; set; }
}