using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
namespace Actio.Services.Activities.Repositories;

public class AtivityRepository(IMongoDatabase mongoDatabase) : IActivityRepository
{
    public async Task<Activity> GetAsync(Guid id)
        => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Activity activity)
        => await Collection.InsertOneAsync(activity);
    
    private IMongoCollection<Activity> Collection
        => mongoDatabase.GetCollection<Activity>("Activities");
}