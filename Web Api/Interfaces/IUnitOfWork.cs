namespace Web_Api.Interfaces;

public interface IUnitOfWork
{
    Task SaveAndCommitAsync(CancellationToken cancellationToken);
}