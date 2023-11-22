using giks.OnlineStore.CustomerService.Domain.Customers;

namespace giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;

public record UpdateCustomerRequestDto(
    Customer Customer);