using System.Text.RegularExpressions;
using giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

namespace giks.OnlineStore.CustomerService.Domain.Customers;

public sealed class CustomerLastName
{
    public static CustomerLastName Create(string lastName)
    {
        return new CustomerLastName(lastName);
    }

    public string LastName => _lastName;

    public static bool Validate(string lastName)
    {
        return Regex.IsMatch(lastName, _nameRegex);
    }

    public bool Equals(CustomerLastName other) => _lastName == other.LastName;

    private string _lastName;
    
    private CustomerLastName(string lastName)
    {
        if (!Validate(lastName))
            throw new NotValidLastNameException();
            
        _lastName = lastName;
    }

    private const string _nameRegex = "([A-Z][a-zA-Z]*)";
}