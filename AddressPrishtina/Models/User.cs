using Microsoft.AspNetCore.Identity;

namespace AddressPrishtina.Models;

public class User : IdentityUser
{
    public RoleType Role { get; set; }
    public List<Address> Addresses { get; set; } = [];
}