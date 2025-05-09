﻿using Asp.Versioning;
using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace RetailAPI.Controllers;
//[Authorize(Roles = "Admin,User")]
[Produces("application/json")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    public UserController(IUserService userService, IUnitOfWork unitOfWork)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO requestDTO)
    {
        try
        {
            if (requestDTO == null)
            {
                return BadRequest("User data must not be null");
            }
            var result = await _userService.CreateUser(requestDTO);
            return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful, Data = result.Data });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin,User")]
    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO dto)
    {
        try
        {
            if (dto == null)
            {
                return BadRequest("User data must not be null");
            }
            await _userService.UpdateUser(dto);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });

        }
    }
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string otp)
    {
        try
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                return BadRequest("Email and OTP must not be null or empty");
            }
            var result = await _userService.VerifyEmail(email, otp);
            return Ok(new ResponseModel { Message = result, Status = APIStatus.Successful });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetUserbyId")]
    public async Task<IActionResult> GetUserbyId([FromQuery] Guid userId)
    {
        try
        {
            var user = await _unitOfWork.Users.GetById(userId);
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data =user });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = users });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
    [HttpPost("ResentOTP")]
    public async Task<IActionResult> ResentOTP([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email must not be null or empty");
            }
            var result = await _userService.ResentOTP(email);
            return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful });
        }
        catch (Exception ex)
        {
            return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
        }
    }
}
