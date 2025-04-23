using MODEL.ApplicationConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Entities;

public class User : Common
{
    [Key]
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string? OTP { get; set; }
    public string? Status { get; set; } = "N";
    public DateTime OTP_Exp { get; set; }
    public Guid? RoleId { get; set; } = new Guid("E2A3172D-8E4A-4B98-8CFA-1F25B0ED0A31");
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? LastAcvite { get; set; }
}
