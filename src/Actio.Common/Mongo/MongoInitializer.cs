using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Actio.Common.Mongo;

public class MongoInitializer:IDatabaseInitializer
{
    private bool _initialized;
    private readonly bool _seed;
    private readonly IMongoDatabase _database;
    private readonly IDatabaseSeeder _databaseSeeder;

    public MongoInitializer(IMongoDatabase database, 
        IDatabaseSeeder databaseSeeder,
        IOptions<MongoOptions> options)
    {
        _database = database;
        _databaseSeeder = databaseSeeder;
        _seed = options.Value.Seed ?? false;
    }
    public async Task InitializeAsync()
    {
        if (_initialized)
            return;
        RegisterConventions();
        _initialized = true;
        if (!_seed)
        {
            return;
        }
        await _databaseSeeder.SeedAsync();
    }

    private void RegisterConventions()
    {
        ConventionRegistry.Register("ActioConventions", new MongoConvention(), x => true);
    }
    private class MongoConvention:IConventionPack
    {
        public IEnumerable<IConvention> Conventions => new List<IConvention>()
        {
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
            new CamelCaseElementNameConvention()
        };
    }
}