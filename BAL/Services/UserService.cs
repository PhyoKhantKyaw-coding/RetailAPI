using BAL.IServices;
using BAL.Shared;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;
using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace BAL.Services;

internal class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    //public async Task CreateUser(CreateUserDTO dto)
    //{
    //    if (dto == null)
    //    {
    //        throw new ArgumentNullException(nameof(dto), "User data must not be null");
    //    }
    //    CommonAuthentication.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
    //    var Roles = await _unitOfWork.Roles.GetByCondition(x => x.RoleName == "User");
    //    Guid roleId;
    //    if (Roles == null)
    //    {
    //        var userRole = new Role
    //        {
    //            RoleId = Guid.NewGuid(),
    //            RoleName = "User",
    //            CreatedAt = DateTime.UtcNow,
    //            UpdatedAt = DateTime.UtcNow
    //        };
    //        await _unitOfWork.Roles.Add(userRole);
    //        await _unitOfWork.SaveChangesAsync();
    //        roleId = userRole.RoleId;
    //    }
    //    else
    //    {
    //        roleId = Roles.FirstOrDefault().RoleId;
    //    }
    //    var user = new User
    //    {
    //        UserId = Guid.NewGuid(),
    //        Email = dto.Email,
    //        RoleId = roleId,
    //        PasswordHash = passwordHash,
    //        PasswordSalt = passwordSalt,
    //        Name = dto.Name,
    //        PhoneNumber = dto.PhoneNumber,
    //        CreatedAt = DateTime.UtcNow,
    //        UpdatedAt = DateTime.UtcNow
    //    };
    //    await _unitOfWork.Users.Add(user);
    //    await _unitOfWork.SaveChangesAsync();
    //}
    public async Task UpdateUser(UpdateUserDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto), "User data must not be null");
        }
        var user = await _unitOfWork.Users.GetById(dto.UserId);
        if (user == null)
        {
            throw new Exception($"User not found with ID: {dto.UserId}");
        }
        if(!string.IsNullOrWhiteSpace(dto.Email))
        {
            user.Email = dto.Email;
        }
        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            user.Name = dto.Name;
        }
        if(!string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            user.PhoneNumber = dto.PhoneNumber;
        }
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<UserResponseDTO> CreateUser(CreateUserDTO requestDTO)
    {
        UserResponseDTO model = new UserResponseDTO();
        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(requestDTO.Email))
        {
            model.IsSuccess = false;
            model.Message = "Invalid email format.";
            return model;
        }
        var phoneRegex = new Regex(@"^\+?[0-9]{9,15}$");
        if (!phoneRegex.IsMatch(requestDTO.PhoneNumber ?? ""))
        {
            model.IsSuccess = false;
            model.Message = "Invalid phone number format.";
            return model;
        }
        var existingUser =( await _unitOfWork.Users.GetByCondition(x => x.Email == requestDTO.Email)).FirstOrDefault();
        if (existingUser !=null )
        {
            model.IsSuccess = false;
            model.Message = "User already exists. Register Failed";
            model.Data = existingUser;
            return model;
        }

        var otpCode = GenerateOTP();
        CommonAuthentication.CreatePasswordHash(requestDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var Roles = (await _unitOfWork.Roles.GetByCondition(x => x.RoleName == "User")).FirstOrDefault();
        Guid roleId;
        if (Roles == null)
        {
            var userRole = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Roles.Add(userRole);
            await _unitOfWork.SaveChangesAsync();
            roleId = userRole.RoleId;
        }
        else
        {
            roleId = Roles.RoleId;
        }
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = requestDTO.Email,
            RoleId = roleId,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Name = requestDTO.Name,
            PhoneNumber = requestDTO.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            OTP = otpCode,
            OTP_Exp = DateTime.Now.AddMinutes(5),
            Status = "N"
        };
        await _unitOfWork.Users.Add(user);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result > 0)
        {
            bool emailSent = SendOTPEmail(user.Email, user.Name, otpCode);
            if (!emailSent)
            {
                model.IsSuccess = false;
                model.Message = "User registered but failed to send OTP email.";
                model.Data = user;
                return model;
            }

            model.IsSuccess = true;
            model.Message = "User Register Successful. OTP sent to email.";
            model.Data = user;
        }
        else
        {
            model.IsSuccess = false;
            model.Message = "User Register Failed";
            model.Data = user;
        }
        return model;
    }

    //private static string HashPassword(string password)
    //{
    //    using SHA256 sha256 = SHA256.Create();
    //    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    //    return Convert.ToBase64String(hashedBytes);
    //}

    private static string GenerateOTP()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static bool SendOTPEmail(string toEmail, string userName, string otpCode)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ddd420698@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Your OTP Code from Phyo Khant Kyaw";


            string htmlBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 500px; margin: auto; background-color: #f9f9f9;'>
                <h2 style='color: #007bff; text-align: center;'>Your OTP Code</h2>
                <p style='font-size: 16px; color: #333;'>Dear <strong>{userName}</strong>,</p>
                <p style='font-size: 16px; color: #333;'>Your One-Time Password (OTP) for verification is:</p>
                <p style='font-size: 24px; font-weight: bold; color: #28a745; text-align: center; padding: 10px; border: 2px dashed #28a745; display: inline-block;'>{otpCode}</p>
                <p style='font-size: 14px; color: #ff0000; text-align: center;'>This OTP will expire in 5 minutes.</p>
               
                <br>
                <p style='font-size: 14px; color: #666; text-align: center;'>Best regards,</p>
                <p style='font-size: 14px; color: #666; text-align: center;'><strong>Retail</strong></p>
            </div>";

            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                //Credentials = new NetworkCredential("acetravelagency.net@gmail.com", "tkdm txbp kkaa lagm"),
                Credentials = new NetworkCredential("ddd420698@gmail.com", "aduu rttx cmsc yhwh"),
                EnableSsl = true
            };

            smtpClient.Send(mail);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> VerifyEmail(string email, string otp)
    {
        var user = (await _unitOfWork.Users.GetByCondition(x => x.Email == email && x.OTP == otp && x.Status == "N")).FirstOrDefault();
        string message;
        if (user == null)
        {
            message = "Invalid OTP or email.";
            return message;
        }
        if (user.OTP_Exp < DateTime.Now)
        {
            message = "OTP expired.";
            return message;
        }
        user.Status = "Y";
        user.OTP = null;
        user.OTP_Exp = DateTime.MinValue;
        user.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        message = "Email verified successfully.";
        return message;
    }
    public async Task<UserResponseDTO> ResentOTP(string email)
    {
        UserResponseDTO model = new UserResponseDTO();
        var user = (await _unitOfWork.Users.GetByCondition(x => x.Email == email && x.Status == "N")).FirstOrDefault();
        if (user == null)
        {
            model.IsSuccess = false;
            model.Message = "User not found or already verified.";
            return model;
        }
        var otpCode = GenerateOTP();
        user.OTP = otpCode;
        user.OTP_Exp = DateTime.Now.AddMinutes(5);
        await _unitOfWork.SaveChangesAsync();
        bool emailSent = SendOTPEmail(user.Email, user.Name, otpCode);
        if (!emailSent)
        {
            model.IsSuccess = false;
            model.Message = "Failed to send OTP email.";
            return model;
        }
        model.IsSuccess = true;
        model.Message = "New OTP sent to email.";
        return model;
    }

}
