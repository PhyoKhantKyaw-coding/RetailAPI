using Microsoft.AspNetCore.Http;
using MODEL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServices;

public interface IProductService
{
    Task AddProduct(AddProductDTO product, IFormFile? imageFile);
    Task UpdateProduct(UpdateProductDTO product, IFormFile? imageFile);
    Task UpdateProduct1(UpdateProductDTO product);
    Task DeleteProduct(Guid productId);
}
