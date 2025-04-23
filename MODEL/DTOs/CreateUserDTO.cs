using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class CreateUserDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
public class UpdateUserDTO
{
    public Guid UserId { get; set; }
    public string? Email { get; set; } 
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; } 
}
public class UserResponseDTO
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public object Data { get; set; } = null!;
}