﻿namespace Core.Interfaces;

public interface IEntity<T>
{
    public T Id { get; init; }
}