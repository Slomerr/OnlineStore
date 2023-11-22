using giks.OnlineStore.CustomerService.Domain.Customers;

namespace giks.OnlineStore.CustomerService.Application.Customers.HasEmail;

public record HasEmailRequestDto(
    CustomerEmail Email);