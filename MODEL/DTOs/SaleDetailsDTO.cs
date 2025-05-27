using MODEL.ApplicationConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class SaleDetailsDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
public class SaleDetailsResponseDTO
{
    public Guid SDId { get; set; }
    public int Quantity { get; set; }
    public String? ProductName { get; set; }
    public string? CatName { get; set; }
    public string? Description { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalCost { get; set; }
}