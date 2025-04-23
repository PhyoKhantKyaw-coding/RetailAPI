using MODEL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServices;

public interface IProductService
{
    Task AddProduct(AddProductDTO product);
    Task UpdateProduct(UpdateProductDTO product);
    Task UpdateProduct1(UpdateProductDTO product);
}
