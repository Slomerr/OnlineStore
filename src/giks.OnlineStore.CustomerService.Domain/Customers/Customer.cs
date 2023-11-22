namespace giks.OnlineStore.CustomerService.Domain.Customers;

public sealed class Customer
{
    public static Customer Create(
        long id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        return new Customer(
            id,
            CustomerFirstName.Create(firstName),
            CustomerLastName.Create(lastName),
            CustomerEmail.Create(email),
            CustomerPhoneNumber.Create(phoneNumber));
    }
    
    public long Id => _id;
    public string FirstName => _firstName.FirstName;
    public string LastName => _lastName.LastName;
    public string Email => _email.Email;
    public string PhoneNumber => _phoneNumber.PhoneNumber;

    public bool EqualsFields(Customer customer)
    {
        return customer._email.Equal(_email) &&
               customer._firstName.Equals(_firstName) &&
               customer._lastName.Equals(_lastName) &&
               customer._phoneNumber.Equals(_phoneNumber);
    }

    private long _id;
    private CustomerFirstName _firstName;
    private CustomerLastName _lastName;
    private CustomerEmail _email;
    private CustomerPhoneNumber _phoneNumber;

    private Customer(
        long id, 
        CustomerFirstName firstName, 
        CustomerLastName lastName, 
        CustomerEmail email, 
        CustomerPhoneNumber phoneNumber)
    {
        _id = id;
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
    }
}