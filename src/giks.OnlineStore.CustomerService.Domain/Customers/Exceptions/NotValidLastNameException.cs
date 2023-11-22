using System.Runtime.Serialization;

namespace giks.OnlineStore.CustomerService.Domain.Customers.Exceptions;

public sealed class NotValidLastNameException : Exception
{
    public NotValidLastNameException()
    {
    }

    public NotValidLastNameException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}