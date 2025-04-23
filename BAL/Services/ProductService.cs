using AutoMapper;
using BAL.IServices;
using Microsoft.EntityFrameworkCore.Metadata;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services;

internal class ProductService: IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task AddProduct(AddProductDTO product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product must not be null");
        }
        var productEntity = new Product
        {
            ProductId = Guid.NewGuid(),
            ProductName = product.ProductName,
            Price = product.Price,
            Cost = product.Cost,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ActiveFlag = true
        };
        await _unitOfWork.Products.Add(productEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateProduct(UpdateProductDTO product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product must not be null");
        }

        var productEntity = await _unitOfWork.Products.GetById(product.ProductId);
        if (productEntity == null)
        {
            throw new Exception($"Product not found with ID: {product.ProductId}");
        }

        if (!string.IsNullOrWhiteSpace(product.ProductName))
        {
            productEntity.ProductName = product.ProductName.Trim();
        }

        if (product.Stock > 0)
        {
            productEntity.Stock = product.Stock;
        }

        if (product.Price > 0)
        {
            productEntity.Price = product.Price;
        }

        if (product.Cost > 0)
        {
            productEntity.Cost = product.Cost;
        }

        productEntity.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Products.Update(productEntity);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateProduct1(UpdateProductDTO product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product must not be null");
        }

        var productEntity = await _unitOfWork.Products.GetById(product.ProductId);
        if (productEntity == null)
        {
            throw new Exception($"Product not found with ID: {product.ProductId}");
        }

        // Map non-default values from DTO to entity
        _mapper.Map(product, productEntity);

        // Always update UpdatedAt timestamp
        productEntity.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Products.Update(productEntity);
        await _unitOfWork.SaveChangesAsync();
    }

}
