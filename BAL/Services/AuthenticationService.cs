using BAL.IServices;
using BAL.Shared;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services;

internal class AuthenticationService:IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CommonTokenGenerator _commonTokenGenerator;
    public AuthenticationService(IUnitOfWork unitOfWork, CommonTokenGenerator commonTokenGenerator = null)
    {
        _unitOfWork = unitOfWork;
        _commonTokenGenerator = commonTokenGenerator;
    }

    public async Task<ResponseUserLoginDTO> LoginWeb(UserLoginDTO loginDTO)
    {
        try
        {
            var returndata = new ResponseUserLoginDTO();
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(loginDTO.Email))
            {
                returndata.EmailStatus = false;
                return returndata;
            }
            var userdata = (await _unitOfWork.Users.GetByCondition(x => x.Email == loginDTO.Email && x.ActiveFlag && x.Status == "Y")).FirstOrDefault();
            if (userdata == null)
            {
                returndata.EmailStatus = false;
                return returndata;
            }
            else
            {
                var roledata = (await _unitOfWork.Roles.GetByCondition(x => x.RoleId == userdata.RoleId)).FirstOrDefault();
                returndata.EmailStatus = true;
                var checkpassword = CommonAuthentication.VerifyPasswordHash(loginDTO.Password, userdata.PasswordHash, userdata.PasswordSalt);
                if (checkpassword && roledata != null)
                {
                    returndata.PasswordStatus = true;
                    returndata.Token = _commonTokenGenerator.Create(userdata,roledata.RoleName);
                    returndata.Email = userdata.Email;
                    returndata.Name = userdata.Name;
                    returndata.RoleName = roledata.RoleName;
                    returndata.RoleId = userdata.RoleId;
                    returndata.UserId = userdata.UserId;
                    returndata.PhoneNumber = userdata.PhoneNumber;
                    returndata.LastActive = userdata.LastAcvite;
                    returndata.CreatedAt = userdata.CreatedAt;
                    returndata.UpdatedAt = userdata.UpdatedAt;
                    return returndata;
                }
                else
                {
                    returndata.PasswordStatus = false;
                    return returndata;
                }

            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    

}
