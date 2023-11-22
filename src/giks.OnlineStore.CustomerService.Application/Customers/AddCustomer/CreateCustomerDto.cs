namespace giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Email,
    string  PhoneNumber);