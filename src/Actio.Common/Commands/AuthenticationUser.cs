namespace Actio.Common.Commands
{
    public record AuthenticationUser(string Email, string Password): ICommand
    {

    }
}