using giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;
using giks.OnlineStore.CustomerService.Application.Customers.GetCustomerById;
using giks.OnlineStore.CustomerService.Application.Customers.HasEmail;
using giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;

namespace giks.OnlineStore.CustomerService.Application.Customers;

public interface ICustomerService
{
    Task<GetCustomerResponseDto> GetCustomer(GetCustomerRequestDto request, CancellationToken token);
    Task<AddCustomerResponseDto> AddCustomer(AddCustomerRequestDto request, CancellationToken token);
    Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto request, CancellationToken token);
    Task<HasEmailResponseDto> HasEmail(HasEmailRequestDto request, CancellationToken token);
}