using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class SaleDTO
{
    public List<SaleDetailsDTO> SaleDetails { get; set; } = new List<SaleDetailsDTO>();
    public Guid UserId { get; set; }
}
public class SaleResponseDTO
{
    public Guid SaleId { get; set; } = Guid.NewGuid();
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public string? UserName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalProfit { get; set; }
    public decimal TotalCost { get; set; }
}