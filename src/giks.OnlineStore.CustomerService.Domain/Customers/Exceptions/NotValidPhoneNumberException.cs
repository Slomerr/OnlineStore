namespace giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

public sealed class NotValidPhoneNumberException : Exception
{
    public NotValidPhoneNumberException()
    {
    }

    public NotValidPhoneNumberException(string? message) : base(message)
    {
    }
}