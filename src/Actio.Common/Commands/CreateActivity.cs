
namespace Actio.Common.Commands
{
    public class CreateActivity() : IAuthenticatedCommand
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; } 
        public required string Category { get; init; } 
        public required string Name { get; init; } 
        public required string Description { get; init; }
        public DateTime? CreatedAt { get; set; }
    }
}