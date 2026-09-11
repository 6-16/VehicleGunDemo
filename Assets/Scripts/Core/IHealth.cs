using System;

public interface IHealth
{
    float Normalized { get; }

    event Action Changed;
}
