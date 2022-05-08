namespace Orders.Core.Shared;

public interface IBuilder<T>
{
    T Build();
}