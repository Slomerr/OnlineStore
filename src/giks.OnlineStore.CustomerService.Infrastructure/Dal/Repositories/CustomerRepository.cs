using giks.OnlineStore.CustomerService.Application.Customers;
using giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;
using giks.OnlineStore.CustomerService.Application.Customers.GetCustomerById;
using giks.OnlineStore.CustomerService.Application.Customers.HasEmail;
using giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;

namespace giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly IAdapterDbConnectionFactory<long> _connectionFactory;

    public CustomerRepository(
        IAdapterDbConnectionFactory<long> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<GetCustomerResponseDto> GetCustomer(GetCustomerRequestDto request, CancellationToken token)
    {        
        throw new NotImplementedException();
    }

    public Task AddCustomer(AddCustomerRequestDto request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<HasEmailResponseDto> HasEmail(HasEmailRequestDto request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}