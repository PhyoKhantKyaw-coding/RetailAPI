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
public class SaleDetailsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleDetailService _saleDetailService;
    public SaleDetailsController(IUnitOfWork unitOfWork,ISaleDetailService saleDetailService)
    {
        _unitOfWork = unitOfWork;
        _saleDetailService = saleDetailService ;
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
    [HttpGet("GetSaleDetailBySaleId")]
    public async Task<IActionResult> GetSaleDetailBySaleId(Guid saleId)
    {
        try
        {
            var result = await _saleDetailService.GetSaleDetailsBySaleId(saleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }

}
