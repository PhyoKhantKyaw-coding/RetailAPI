using Asp.Versioning;
using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class Productv2Controller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public Productv2Controller(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product data must not be null");
                }
                await _productService.UpdateProduct1(product);
                return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
    }
}
