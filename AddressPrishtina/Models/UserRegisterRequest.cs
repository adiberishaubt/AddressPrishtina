using System.ComponentModel.DataAnnotations;

namespace AddressPrishtina.Models;

public class UserRegisterRequest
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password should be 8+ characters long!")]
    public string Password { get; set; }
    [RegularExpression(@"\+383\d{3}\d{3}")]
    public string PhoneNumber { get; set; }

    public RoleType Role { get; set; }
}