namespace Web_Api.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task SaveAndCommitAsync(CancellationToken cancellationToken);
}