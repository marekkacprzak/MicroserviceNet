namespace Actio.Common.Events;

public interface IRejectEvent:IEvent
{
    string Reason {get; init;}
    string Code {get; init;}
}
