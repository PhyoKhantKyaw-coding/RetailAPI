using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MODEL.ApplicationConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Entities;
[Table("TBL_Product")]
public class Product:Common
{
    [Key]
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public String? ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public decimal Profit => Price - Cost;
    public string? image { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; }

}
