using Actio.Common.Auth;
using Actio.Services.Activities.Exceptions;
using Actio.Services.Identity.Domain.Repositories;
using Actio.Services.Identity.Domain.Services;

namespace Actio.Services.Identity.Services;

public class UserService(IUserRepository userRepository, 
    IEncrypter encrypter,
    IJwtHandler jwtHandler) : IUserService
{
    public async Task RegisterAsync(string email, string password, string name)
    {
        var user = await userRepository.GetAsync(email);
        if (user != null)
        {
            throw new ActioException("email_is_use",
                $"Email: {email} is already in use");
        }

        user = new(email, name);
        user.SetPassword(password, encrypter);
        await userRepository.AddAsync(user);
    }

    public async Task<JsonWebToken> LoginAsync(string email, string password)
    {
        var user = await userRepository.GetAsync(email);
        if (user == null)
        {
            throw new ActioException("invalid_credential",
                $"Invalid credential.");
        }

        if (!user.ValidatePassword(password, encrypter))
        {
            throw new ActioException("invalid_credential",
                $"Invalid credential.");
        }
        return jwtHandler.Create(user.Id);
    }
}