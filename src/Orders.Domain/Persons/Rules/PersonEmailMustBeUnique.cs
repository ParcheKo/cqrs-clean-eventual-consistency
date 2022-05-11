using Orders.Domain.SeedWork;

namespace Orders.Domain.Persons.Rules;

public class PersonEmailMustBeUnique : IBusinessRule
{
    private readonly bool _isUnique;

    public PersonEmailMustBeUnique(bool isUnique)
    {
        _isUnique = isUnique;
    }

    public bool IsBroken()
    {
        return !_isUnique;
    }

    public string Message => "Person with this email already exists.";
}