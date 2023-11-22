using System.Text.RegularExpressions;
using giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

namespace giks.OnlineStore.CustomerService.Domain.Customers;

public sealed class CustomerFirstName
{
    public static CustomerFirstName Create(string firstName)
    {
        return new CustomerFirstName(firstName);
    }

    public string FirstName => _firstName;

    public static bool Validate(string firstName)
    {
        return Regex.IsMatch(firstName, _nameRegex);
    }

    public bool Equals(CustomerFirstName other) => _firstName == other.FirstName;

    private string _firstName;

    private CustomerFirstName(string firstName)
    {
        if (!Validate(firstName))
            throw new NotValidFirstNameException();
            
        _firstName = firstName;
    }

    private const string _nameRegex = "([A-Z][a-zA-Z]*)";
}