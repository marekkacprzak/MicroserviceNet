using Actio.Services.Activities.Exceptions;
using Actio.Services.Identity.Domain.Services;

namespace Actio.Services.Identity.Domain.Models;

public record User(Guid Id, string Email, string Name, DateTime CreatedAt)
{
    public string Password { get; private set; } = "";
    public string Salt { get; private set; } = "";
    public User(string email, string name):this(Guid.NewGuid(),email,name,DateTime.UtcNow)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ActioException("empty_user_email",
                $"User email can not be empty.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ActioException("empty_user_name",
                $"User name can not be empty.");

    }

    public void SetPassword(string password, IEncrypter encrypter)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ActioException("empty_password", "User password not set");
        Salt = encrypter.GetSalt(password);
        Password = encrypter.GetHash(password, Salt);
    }

    public bool ValidatePassword(string password, IEncrypter encrypter)
        => Password.Equals(encrypter.GetHash(password, Salt));
}