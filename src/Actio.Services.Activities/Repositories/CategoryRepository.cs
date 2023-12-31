using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Actio.Services.Activities.Repositories;

public class CategoryRepository(IMongoDatabase mongoDatabase) : ICategoryRepository
{
    public async Task<Category> GetAsync(string name)
        => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Name == name.ToLowerInvariant());

    public async Task<IEnumerable<Category>> BrowseAsync()
        => await Collection
            .AsQueryable()
            .ToListAsync();

    public async Task AddAsync(Category category)
        => await Collection.InsertOneAsync(category);

    private IMongoCollection<Category> Collection
        => mongoDatabase.GetCollection<Category>("Categories");
}