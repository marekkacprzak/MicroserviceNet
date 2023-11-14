using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Activity = Actio.Api.Models.Activity;

namespace Actio.Api.Repositories;

public class ActivityRepository(IMongoDatabase mongoDatabase) : IActivityRepository
{

    public async Task<Models.Activity> GetAsync(Guid id)
        => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Activity model)
        => await Collection.InsertOneAsync(model);

    public async Task<IEnumerable<Activity>> BrowseAsync(Guid userId)
        => await Collection.AsQueryable().Where(x => x.UserId == userId).ToListAsync();

    private IMongoCollection<Activity> Collection
        => mongoDatabase.GetCollection<Activity>("Activities");
}