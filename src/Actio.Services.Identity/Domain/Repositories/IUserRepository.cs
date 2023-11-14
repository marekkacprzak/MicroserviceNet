using Actio.Services.Identity.Domain.Models;

namespace Actio.Services.Identity.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetAsync();
    Task<User> GetAsync(Guid id);
    Task<User> GetAsync(string email);
    Task AddAsync(User user);
}