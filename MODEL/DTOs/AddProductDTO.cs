using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class AddProductDTO
{
    public String? ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}
public class UpdateProductDTO
{
    public Guid ProductId { get; set; }
    public String? ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}
