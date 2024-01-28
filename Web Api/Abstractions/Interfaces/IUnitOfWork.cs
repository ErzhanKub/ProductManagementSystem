namespace WebApi.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task SaveAndCommitAsync(CancellationToken cancellationToken);
}