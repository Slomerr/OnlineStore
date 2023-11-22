namespace giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

public sealed class NotValidEmailException : Exception
{
    public NotValidEmailException()
    {
    }

    public NotValidEmailException(string? message) : base(message)
    {
    }
}