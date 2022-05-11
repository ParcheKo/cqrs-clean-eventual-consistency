namespace Orders.Domain.SeedWork;

public interface IBusinessRule
{
    string Message { get; }
    bool IsBroken();
}