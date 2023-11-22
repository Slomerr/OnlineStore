using System.Text.RegularExpressions;
using giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

namespace giks.OnlineStore.CustomerService.Domain.Customers;

public sealed class CustomerEmail
{
    public static CustomerEmail Create(string email)
    {
        return new CustomerEmail(email);
    }

    public string Email => _email;

    public static bool Validate(string email)
    {
        return Regex.IsMatch(email, _emailRegex);
    }

    public bool Equal(CustomerEmail other) => other.Email == _email;

    private string _email;
    
    private CustomerEmail(string email)
    {
        if (!Validate(email))
            throw new NotValidEmailException();
        
        _email = email;
    }

    private const string _emailRegex = @"^\\S+@\\S+\\.\\S+$";
}