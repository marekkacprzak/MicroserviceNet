using MongoDB.Driver;

namespace Actio.Common.Mongo;

public class MongoSeeder:IDatabaseSeeder
{
    private readonly IMongoDatabase MongoDatabase;

    public MongoSeeder(IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase;
    }
    public async Task SeedAsync()
    {
        var collCursor = await MongoDatabase.ListCollectionsAsync();
        var collections = await collCursor.ToListAsync();
        if (collections.Any())
        {
            return;
        }

        await CustomSeedAsync();
    }

    protected virtual async Task CustomSeedAsync()
    {
        await Task.CompletedTask;
    }
}