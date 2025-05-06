using MODEL.DTOs;

namespace BAL.IServices;

public interface IUserService
{
    Task<UserResponseDTO> CreateUser(CreateUserDTO requestDTO);
    Task UpdateUser(UpdateUserDTO dto);
    Task<string> VerifyEmail(string email, string otp);
    Task<UserResponseDTO> ResentOTP(string email);
}