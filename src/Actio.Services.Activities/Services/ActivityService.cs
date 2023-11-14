using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Exceptions;

namespace Actio.Services.Activities.Services;

public class ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository):IActivityService
{
    public async Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdAt)
    {
        var activityCategory = await categoryRepository.GetAsync(category);
        if (activityCategory == null)
            throw new ActioException("category_not_found",
                $"Category: '{category} was not found");
        var activity = new Activity(id, category, userId, name, description, createdAt);
        await activityRepository.AddAsync(activity);
    }
}