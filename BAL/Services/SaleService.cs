﻿using BAL.IServices;
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

internal class SaleService : ISaleService
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


    public async Task<List<SaleResponseDTO>> GetSales()
    {
        var sales = await _unitOfWork.Sales.GetAll();
        List<SaleResponseDTO> result = new List<SaleResponseDTO>();
        foreach(var sale in sales)
        {
            var userData = await _unitOfWork.Users.GetById(sale.UserId);
            result.Add(new SaleResponseDTO
            {
                UserName = userData.Name,
                SaleId =sale.SaleId,
                SaleDate = sale.SaleDate,
                TotalAmount = sale.TotalAmount,
                TotalCost = sale.TotalCost,
                TotalProfit = sale.TotalProfit,
            });
        }
        return result;
    }
    public async Task<List<SaleResponseDTO>> GetSalesByUserId(Guid userId)
    {
        var result = (from sale in await _unitOfWork.Sales.GetAll()
                            join user in await  _unitOfWork.Users.GetAll()
                            on sale.UserId equals user.UserId
                            where sale.UserId == userId
                            select new SaleResponseDTO
                            {
                                SaleId = sale.SaleId,
                                SaleDate = sale.SaleDate,
                                UserName = user.Name,
                                TotalAmount = sale.TotalAmount,
                                TotalProfit = sale.TotalProfit,
                                TotalCost = sale.TotalCost
                            }).ToList();
        return result;
    }

}
