using Asp.Versioning;
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
public class SaleDetailsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public SaleDetailsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [Authorize(Roles = "Admin,User")]
    [HttpGet("GetAllSalesDatail")]
    public async Task<IActionResult> GetAllSalesDatail()
    {
        try
        {
            var sales = await _unitOfWork.SaleDetail.GetAll();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = sales });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetSaleDetailBySaleId")]
    public async Task<IActionResult> GetSaleDetailBySaleId(Guid saleId)
    {
        try
        {
            var sales = await _unitOfWork.SaleDetail.GetByCondition(x => x.SaleId == saleId);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = sales });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

}
