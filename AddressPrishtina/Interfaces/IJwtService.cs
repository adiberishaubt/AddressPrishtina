using System.Security.Claims;

namespace AddressPrishtina.Interfaces;

public interface IJwtService
{
    string GenerateJwt(List<Claim> claims);
}