using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Actio.Services.Identity.Repositories;

public class UserRepository(IMongoDatabase mongoDatabase):IUserRepository
{
    public async Task<User> GetAsync()
        => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync();
    public async Task<User> GetAsync(Guid id)
        => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User> GetAsync(string email)
        => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(User user)
        => await Collection.InsertOneAsync(user);
    
    private IMongoCollection<User> Collection
        => mongoDatabase.GetCollection<User>("Users");
}