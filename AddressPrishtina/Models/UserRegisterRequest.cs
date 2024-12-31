using System.ComponentModel.DataAnnotations;

namespace AddressPrishtina.Models;

public class UserRegisterRequest
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    [RegularExpression(@"^.{8,}$", ErrorMessage = "Password should be 8 or more characters long!")]
    public string Password { get; set; }
    [RegularExpression(@"\+383\d{3}\d{3}")]
    public string PhoneNumber { get; set; }

    public RoleType Role { get; set; }
}