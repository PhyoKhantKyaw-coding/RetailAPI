using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MODEL.Entities;

public class Category
{
    [Key]
    public Guid CategoryId { get; set; }
    public string? Name { get; set; }
}
