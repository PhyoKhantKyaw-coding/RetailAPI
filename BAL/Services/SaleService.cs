using BAL.IServices;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services;

internal class SaleService: ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    public SaleService(IUnitOfWork unitOfWork, IProductService productService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }
    public async Task<ResponseModel> AddSale(SaleDTO sale)
    {
        if (sale == null || sale.SaleDetails == null || !sale.SaleDetails.Any())
        {
            return new ResponseModel
            {
                Message = Messages.InvalidPostedData,
                Status = APIStatus.Error
            };
        }

        try
        {
            decimal totalAmount = 0;
            decimal totalCost = 0;
            var saleId = Guid.NewGuid();
            var saleDate = DateTime.UtcNow;

            var saleDetailsEntities = new List<SaleDetails>();

            foreach (var detailDto in sale.SaleDetails)
            {
                var product = (await _unitOfWork.Products.GetByCondition(x => x.ProductId == detailDto.ProductId && x.ActiveFlag))
                              .FirstOrDefault();

                if (product == null)
                {
                    return new ResponseModel
                    {
                        Message = $"Product not found with ID: {detailDto.ProductId}",
                        Status = APIStatus.Error
                    };
                }

                if (detailDto.Quantity == 0)
                {
                    return new ResponseModel
                    {
                        Message = $"Requested quantity is zero for product ID: {detailDto.ProductId}",
                        Status = APIStatus.Error
                    };
                }

                if (product.Stock < detailDto.Quantity)
                {
                    return new ResponseModel
                    {
                        Message = $"Insufficient stock for product ID: {detailDto.ProductId}. Requested: {detailDto.Quantity}",
                        Status = APIStatus.Error
                    };
                }

                var detailTotalPrice = product.Price * detailDto.Quantity;
                var detailTotalCost = product.Cost * detailDto.Quantity;

                totalAmount += detailTotalPrice;
                totalCost += detailTotalCost;

                saleDetailsEntities.Add(new SaleDetails
                {
                    SDId = Guid.NewGuid(),
                    SaleId = saleId,
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    Price = product.Price,
                    TotalPrice = detailTotalPrice,
                    TotalCost = detailTotalCost,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ActiveFlag = true
                });

                product.Stock -= detailDto.Quantity;
                _unitOfWork.Products.Update(product); // no need to await
            }

            var saleEntity = new Sale
            {
                SaleId = saleId,
                SaleDate = saleDate,
                UserId = sale.UserId,
                TotalAmount = totalAmount,
                TotalCost = totalCost,
                TotalProfit = totalAmount - totalCost
            };

            await _unitOfWork.Sales.Add(saleEntity);
            await _unitOfWork.SaleDetail.AddMultiple(saleDetailsEntities);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                Message = Messages.AddSucess,
                Status = APIStatus.Successful,
                Data = new { SaleId = saleId }
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel
            {
                Message = ex.Message,
                Status = APIStatus.SystemError
            };
        }
    }



}
