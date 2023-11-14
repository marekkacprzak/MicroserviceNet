namespace Actio.Common.Commands
{
    public record CreateUser(string Email, string Password, string Name):ICommand
    {
    }
}