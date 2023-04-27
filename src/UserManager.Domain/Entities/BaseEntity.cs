namespace UserManager.Domain.Entities;

public abstract class BaseEntity<TKey>
    where TKey : notnull
{
    public TKey Id { get; protected init; } = default!;
}