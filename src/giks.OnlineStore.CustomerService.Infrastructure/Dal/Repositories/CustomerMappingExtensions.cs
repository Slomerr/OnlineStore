using giks.OnlineStore.CustomerService.Domain.Customers;
using giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories.Dtos;

namespace giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories;

internal static class CustomerMappingExtensions
{
    public static Customer MapToDomain(this CustomerDto dto)
    {
        return Customer.Create(
            dto.Id,
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.PhoneNumber);
    }

    public static CustomerDto MapFromDomain(this Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber);
    }
}