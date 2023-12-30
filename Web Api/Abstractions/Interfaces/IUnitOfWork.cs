namespace WebApi.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task<int> Complete(CancellationToken cancellationToken);
    void Dispose();
}