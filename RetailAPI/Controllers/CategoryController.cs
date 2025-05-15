using Asp.Versioning;
using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork, IProductService productService)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpPost]
    public async Task<IActionResult> AddCat(string name)
    {
        try
        {
            var cat = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = name,
            };
            await _unitOfWork.Categorys.Add(cat);
            var result = _unitOfWork.SaveChangesAsync();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful,Data=cat });

        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError }); 
        }
    }
    [HttpGet("GetAllCategory")]
    public async Task<IActionResult> GetCategory()
    {
        try
        {
            var cats = await _unitOfWork.Categorys.GetAll();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = cats });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpPatch("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory (Category category)
    {
        try
        {
            var cat = await _unitOfWork.Categorys.GetById(category.CategoryId);
            if (cat == null)
            {
                return Ok(new ResponseModel
                {
                    Message = "Category not found.",
                });
            }
            cat.Name=category.Name;
             _unitOfWork.Categorys.Update(cat);
            var result = await _unitOfWork.SaveChangesAsync();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = result });

        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpGet("GetbyId")]
    public async Task<IActionResult> CategoryGetById (Guid categoryId)
    {
        try
        {
            var cat = await _unitOfWork.Categorys.GetById(categoryId);
            if (cat == null)
            {
                return Ok(new ResponseModel
                {
                    Message = "Category not found.",
                });

            }
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = cat.Name  });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
}
