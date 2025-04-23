using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers;
[Authorize(Roles = "Admin,User")]
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

}
