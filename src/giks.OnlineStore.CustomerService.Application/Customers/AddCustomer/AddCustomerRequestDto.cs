using giks.OnlineStore.CustomerService.Domain.Customers;

namespace giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;

public record AddCustomerRequestDto(
    CreateCustomerDto Customer);