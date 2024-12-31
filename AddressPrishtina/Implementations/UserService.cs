using System.Security.Claims;
using AddressPrishtina.Interfaces;
using AddressPrishtina.Models;
using Microsoft.AspNetCore.Identity;

namespace AddressPrishtina.Implementations;

public class UserService : IUserService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;


    public UserService(IJwtService jwtService, UserManager<User> userManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
    }
    
    public async Task RegisterUserAsync(UserRegisterRequest userRegister, CancellationToken cancellationToken)
    {
        var queryResults = await _userManager.FindByEmailAsync(userRegister.Email);

        if (queryResults != null)
        {
            throw new Exception($"User with email {userRegister.Email} already exists!");
        }

        var userToCreate = new User
        {
            UserName = userRegister.Fullname,
            Email = userRegister.Email,
            PhoneNumber = userRegister.PhoneNumber,
            Role = userRegister.Role
        };

        var createdUser = await _userManager.CreateAsync(userToCreate, userRegister.Password);

        if (createdUser.Succeeded)
        {
            await _userManager.AddToRoleAsync(userToCreate, userRegister.Role.ToString());
        }
        else
        {
            throw new Exception(string.Join("\n", createdUser.Errors.Select(e => e.Description)));
        }
    }

    public async Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(userLoginRequest.Email);

        if (user == null)
        {
            throw new Exception("User doesn't exist!");
        }

        var result = await _userManager.CheckPasswordAsync(user, userLoginRequest.Password);

        if (result == false)
        {
            throw new Exception("Wrong password!" );
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new LoginResponse
        {
            userId = user.Id,
            jwt = _jwtService.GenerateJwt(claims)
        };
    }
}