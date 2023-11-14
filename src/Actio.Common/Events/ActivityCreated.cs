

namespace Actio.Common.Events;

public record ActivityCreated(Guid Id, Guid UserId, string Category, string Name, string Description, DateTime CreatedAt) : IAuthenticatedEvent
{
}
