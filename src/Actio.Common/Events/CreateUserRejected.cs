using Actio.Common.Events;

namespace Actio.Common;

public record  CreateUserRejected(string Reason, string Code, string Email): IRejectEvent
{

}
