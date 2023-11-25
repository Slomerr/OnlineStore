namespace giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;

public record AddCustomerRequestDto(
    CreateCustomerDto Customer,
    long CreationTime);