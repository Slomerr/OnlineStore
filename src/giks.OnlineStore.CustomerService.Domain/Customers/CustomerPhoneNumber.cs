using System.Text.RegularExpressions;
using giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

namespace giks.OnlineStore.CustomerService.Domain.Customers;

public sealed class CustomerPhoneNumber
{
    public static CustomerPhoneNumber Create(string phoneNumber)
    {
        return new CustomerPhoneNumber(phoneNumber);
    }

    public string PhoneNumber => _phoneNumber;

    public static bool Validate(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, _numberRegex);
    }

    public bool Equals(CustomerPhoneNumber other) => _phoneNumber == other.PhoneNumber;

    private string _phoneNumber;
    
    private CustomerPhoneNumber(string phoneNumber)
    {
        if (!Validate(phoneNumber))
            throw new NotValidPhoneNumberException();
            
        _phoneNumber = phoneNumber;
    }
    
    private const string _numberRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7}$";
}