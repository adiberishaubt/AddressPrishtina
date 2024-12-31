using System.Security.Claims;
using AddressPrishtina.Interfaces;
using AddressPrishtina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressPrishtina.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> CreateUserAsync(UserRegisterRequest createUserRequest, CancellationToken cancellationToken)
    {
        await _userService.RegisterUserAsync(createUserRequest, cancellationToken);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(UserLoginRequest loginRequest, CancellationToken cancellationToken)
    {
        return await _userService.LoginAsync(loginRequest, cancellationToken);
    }

    [Authorize]
    [HttpGet("role")]
    public ActionResult<string> GetRoleAsync()
    {
        return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
    }
}