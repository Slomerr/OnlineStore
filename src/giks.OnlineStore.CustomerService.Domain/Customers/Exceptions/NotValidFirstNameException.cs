using System.Runtime.Serialization;

namespace giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

public sealed class NotValidFirstNameException : Exception
{
    public NotValidFirstNameException()
    {
    }

    public NotValidFirstNameException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}