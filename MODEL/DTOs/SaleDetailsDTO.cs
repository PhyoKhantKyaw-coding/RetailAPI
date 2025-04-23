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
}
