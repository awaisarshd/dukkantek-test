namespace Dukkantek.Test.Application.Common.Interfaces;
public interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
}
