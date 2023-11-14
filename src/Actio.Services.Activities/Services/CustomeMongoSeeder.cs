using Actio.Common.Mongo;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;

namespace Actio.Services.Activities.Services;

public class CustomeMongoSeeder(IMongoDatabase mongoDatabase,
    ICategoryRepository CategoryRepository): MongoSeeder(mongoDatabase)
{
    protected override async Task CustomSeedAsync()
    {
        var category = new List<string>
        {
            "work",
            "sport",
            "hobby"
        };
        await Task.WhenAll(category.Select(c =>
            CategoryRepository.AddAsync(new Category(c))));
    }
}