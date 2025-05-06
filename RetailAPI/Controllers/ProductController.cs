using Asp.Versioning;
using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]

public class ProductController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    public ProductController(IUnitOfWork unitOfWork,IProductService productService)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
    }
    [Authorize(Roles = "Admin,User")]
    [HttpGet("GetAllProducts")]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _unitOfWork.Products.GetAll();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = products });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetProductById")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        try
        {
            var product = await _unitOfWork.Products.GetById(id);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = product });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromForm] AddProductDTO product, IFormFile? imageFile)
    {
        try
        {
            await _productService.AddProduct(product, imageFile);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful});
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO product, IFormFile? imageFile)
    {
        try
        {
            await _productService.UpdateProduct(product);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpGet("GetProductWithPaginationId")]
    public async Task<IActionResult> GetProductWithPaginationId([FromQuery] PaginationDTO pagination)
    {
        try
        {
            var products = await _unitOfWork.Products.GetAllAsyncWithPagination(pagination.page, pagination.pageSize);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = products });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
}
