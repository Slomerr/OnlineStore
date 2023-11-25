namespace giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories.Exceptions;

public sealed class NotFoundCustomerByIdException : Exception
{
    public NotFoundCustomerByIdException(long id) : base($"Not found customer by id {id}")
    {
    }
}