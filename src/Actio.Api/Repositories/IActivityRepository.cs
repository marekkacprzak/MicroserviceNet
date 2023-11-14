using Actio.Api.Models;

namespace Actio.Api.Repositories;

public interface IActivityRepository
{
    Task<Activity> GetAsync(Guid id);
    Task AddAsync(Activity model);
    Task<IEnumerable<Activity>> BrowseAsync(Guid userId);
}