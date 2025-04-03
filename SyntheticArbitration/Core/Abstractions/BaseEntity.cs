using Core.Interfaces;

namespace Core.Abstractions;

public class BaseEntity : IEntity<int>
{
    public int Id { get; init; }
}