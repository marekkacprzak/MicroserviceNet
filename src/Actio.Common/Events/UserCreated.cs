namespace Actio.Common.Events;

public record UserCreated(string Email, string Name):IEvent
{

}
