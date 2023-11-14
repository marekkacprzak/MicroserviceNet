namespace Actio.Common.Events;

public class CreateActivityRejected:IRejectEvent
{
    public Guid Id { get; }
    
    public CreateActivityRejected()
    {
        
    }
    public CreateActivityRejected(Guid id, string exCode, string exMessage)
    {
        Id = id;
        Code = exCode;
        Reason = exMessage;
    }

    public string Reason { get; init; } = "";
    public string Code { get; init; } = "";
}