using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Product
{
    public string ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ShortDesc { get; set; }

    public string? Description { get; set; }

    public string? CatId { get; set; }

    public double? Price { get; set; }

    public string? Unit { get; set; }

    public string? Thumb { get; set; }

    public string? Video { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public bool BestSellers { get; set; }

    public bool HomeFlag { get; set; }

    public bool Active { get; set; }

    public string? Tags { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? MetaDesc { get; set; }

    public string? MetaKey { get; set; }

    public virtual Category? Cat { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
