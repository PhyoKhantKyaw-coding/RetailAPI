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
