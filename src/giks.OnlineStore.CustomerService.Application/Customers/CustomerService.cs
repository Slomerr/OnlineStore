using giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;
using giks.OnlineStore.CustomerService.Application.Customers.GetCustomerById;
using giks.OnlineStore.CustomerService.Application.Customers.HasEmail;
using giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;
using giks.OnlineStore.CustomerService.Domain.Customers;
using Microsoft.Extensions.Logging;

namespace giks.OnlineStore.CustomerService.Application.Customers;

internal sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<ICustomerService> _logger;

    public CustomerService(
        ICustomerRepository repository,
        ILogger<ICustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<GetCustomerResponseDto> GetCustomer(GetCustomerRequestDto request, CancellationToken token)
    {
        _logger.LogDebug("Start getting customer by id {id}", request.CustomerId);
        
        token.ThrowIfCancellationRequested();

        var result = await _repository.GetCustomer(request, token);
        
        _logger.LogDebug("Complete getting customer with result{@customer}", result.Customer);
        return result;
    }

    public async Task<AddCustomerResponseDto> AddCustomer(AddCustomerRequestDto request, CancellationToken token)
    {
        _logger.LogDebug("Start adding customer");
        
        token.ThrowIfCancellationRequested();
        
        if (!ValidateCustomer(request.Customer))
        {
            _logger.LogDebug("Complete adding customer because fields is wrong");
            return new AddCustomerResponseDto(false, "Wrong customer fields");
        }

        var hasEmailRequest = new HasEmailRequestDto(CustomerEmail.Create(request.Customer.Email));
        var hasEmailResponse = await _repository.HasEmail(hasEmailRequest, token);
        if (hasEmailResponse.Has)
        {
            _logger.LogDebug("Complete adding customer because email already exist");
            return new AddCustomerResponseDto(false, "Email already exist");
        }

        token.ThrowIfCancellationRequested();
        
        await _repository.AddCustomer(request, token);
        _logger.LogDebug("Complete adding customer");
        return new AddCustomerResponseDto(true, string.Empty);
    }

    public async Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto request, CancellationToken token)
    {
        _logger.LogDebug("Start update customer with id={id}", request.Customer.Id);
        
        token.ThrowIfCancellationRequested();
        
        var getCustomerRequest = new GetCustomerRequestDto(request.Customer.Id);
        var getCustomerResponse = await _repository.GetCustomer(getCustomerRequest, token);
        if (request.Customer.EqualsFields(getCustomerResponse.Customer))
        {
            _logger.LogDebug("Complete update customer without update repository, because the customer fields are equal to the old once");
            return new UpdateCustomerResponseDto(true);
        }

        token.ThrowIfCancellationRequested();
        
        var response = await _repository.UpdateCustomer(request, token);
        _logger.LogDebug("Complete update customer with result {@result}", response);
        return response;
    }

    public async Task<HasEmailResponseDto> HasEmail(HasEmailRequestDto request, CancellationToken token)
    {
        _logger.LogDebug("Start check email exist");
        
        token.ThrowIfCancellationRequested();

        var result = await _repository.HasEmail(request, token);
        _logger.LogDebug("Complete check email exist");
        return result;
    }

    private bool ValidateCustomer(CreateCustomerDto dto)
    {
        return CustomerFirstName.Validate(dto.FirstName) &&
               CustomerLastName.Validate(dto.LastName) &&
               CustomerEmail.Validate(dto.Email) &&
               CustomerPhoneNumber.Validate(dto.PhoneNumber);
    }
}