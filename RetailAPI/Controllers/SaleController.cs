using Asp.Versioning;
using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class SaleController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleService _saleService;
    public SaleController(IUnitOfWork unitOfWork, ISaleService saleService)
    {
        _unitOfWork = unitOfWork;
        _saleService = saleService;
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllSales")]
    public async Task<IActionResult> GetAllSales()
    {
        try
        {
            var sales = await _saleService.GetSales();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = sales });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
   [Authorize(Roles = "Admin,User")]
    [HttpPost("AddSale")]
    public async Task<IActionResult> AddSale([FromBody] SaleDTO sale)
    {
        try
        {
            var result = await _saleService.AddSale(sale);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "User")]
    [HttpGet("GetSaleByUserId")]
    public async Task<IActionResult> GetSaleByUserId(Guid userId)
    {
        try
        {
            var sales = await _saleService.GetSalesByUserId(userId);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = sales });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
}