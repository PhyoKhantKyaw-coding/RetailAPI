using BAL.IServices;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BAL.Services;

internal class SaleDetailService : ISaleDetailService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    public SaleDetailService(IUnitOfWork unitOfWork, IProductService productService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }
    public async Task<ResponseModel> GetSaleDetailsBySaleId(Guid saleId)
    {
        if (saleId == Guid.Empty)
        {
            throw new ArgumentException("Sale ID must not be empty", nameof(saleId));
        }
        var saleDetails = await _unitOfWork.SaleDetail.GetByCondition(x => x.SaleId == saleId && x.ActiveFlag);
        if (saleDetails == null || !saleDetails.Any())
        {
            return new ResponseModel
            {
                Message = "No sale details found for the provided Sale ID.",
                Status = APIStatus.Error
            };
        }
        List<SaleDetailsResponseDTO> saleDetailsResponse = new List<SaleDetailsResponseDTO>();
        foreach (var item in saleDetails) {
            var product = (await _unitOfWork.Products.GetByCondition(x => x.ProductId == item.ProductId && x.ActiveFlag))
                          .FirstOrDefault();
            if (product == null)
            {
                return new ResponseModel
                {
                    Message = $"Product not found with ID: {item.ProductId}",
                    Status = APIStatus.Error
                };
            }
            var category = (await _unitOfWork.Categorys.GetByCondition(x => x.CategoryId == product.CategoryId))
                          .FirstOrDefault();
            if (category == null)
            {
                return new ResponseModel
                {
                    Message = $"Category not found with ID: {product.CategoryId}",
                    Status = APIStatus.Error
                };
            }

            saleDetailsResponse.Add(new SaleDetailsResponseDTO
            {
                SDId = item.SDId,
                ProductName = product.ProductName,
                CatName = category.Name,
                Quantity = item.Quantity,
                Description = product.Description,
                TotalCost = item.TotalCost,
                TotalPrice = item.TotalPrice
            });
        }
        return new ResponseModel
        {
            Message = Messages.Successfully,
            Status = APIStatus.Successful,
            Data = saleDetailsResponse
        };
    }
}
