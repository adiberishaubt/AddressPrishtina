using AddressPrishtina.Models;

namespace AddressPrishtina.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(UserRegisterRequest userRegister, CancellationToken cancellationToken);
    Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest, CancellationToken cancellationToken);
}