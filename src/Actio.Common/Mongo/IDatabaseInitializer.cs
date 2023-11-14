namespace Actio.Common.Mongo;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}