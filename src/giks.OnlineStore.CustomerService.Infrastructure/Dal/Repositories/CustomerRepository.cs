using System.Data;
using Dapper;
using giks.OnlineStore.CustomerService.Application.Customers;
using giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;
using giks.OnlineStore.CustomerService.Application.Customers.GetCustomerById;
using giks.OnlineStore.CustomerService.Application.Customers.HasEmail;
using giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;
using giks.OnlineStore.CustomerService.Application.Utility;
using giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories.Dtos;
using giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories.Exceptions;
using giks.OnlineStore.Dal.ShardDb.Dal.Common.Connection;
using Microsoft.Extensions.Logging;

namespace giks.OnlineStore.CustomerService.Infrastructure.Dal.Repositories;

internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly IAdapterDbConnectionFactory<long> _adapterConnectionFactory;
    private readonly IShardDbConnectionFactory _connectionFactory;
    private readonly ILogger<CustomerRepository> _logger;
    public CustomerRepository(
        IAdapterDbConnectionFactory<long> adapterConnectionFactory,
        IShardDbConnectionFactory connectionFactory,
        ILogger<CustomerRepository> logger)
    {
        _adapterConnectionFactory = adapterConnectionFactory;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<GetCustomerResponseDto> GetCustomer(
        GetCustomerRequestDto request, 
        CancellationToken token)
    {
        const string sqlRequest = $@"
select {SelectFields}
    from {CustomerTable}
where customer_id = :id
";
        using var connection = _adapterConnectionFactory.GetConnectionByShardKey(request.CustomerId);
        var parameters = new DynamicParameters();
        parameters.Add("id", request.CustomerId);
        var commandDefinition = new CommandDefinition(
            commandText: sqlRequest,
            parameters: parameters,
            cancellationToken: token);
        var customerDto = await connection.QueryFirstOrDefaultAsync<CustomerDto>(commandDefinition);
        if (customerDto == null)
        {
            throw new NotFoundCustomerByIdException(request.CustomerId);
        }

        return new GetCustomerResponseDto(customerDto.MapToDomain());
    }

    public async Task AddCustomer(AddCustomerRequestDto request, CancellationToken token)
    {
        const string sqlAddCustomer = $@"
insert into {CustomerTable} ({InsertFields})
    values (:id, 
            :first_name, 
            :last_name,
            :email,
            :phone_number,
            :date_creation);
";
        var id = await GetId(token);
        var parameters = new DynamicParameters();
        parameters.Add("id", id);
        parameters.Add("first_name",  request.Customer.FirstName);
        parameters.Add("last_name", request.Customer.LastName);
        parameters.Add("email", request.Customer.Email);
        parameters.Add("phone_number", request.Customer.PhoneNumber);
        parameters.Add("date_creation", request.CreationTime);

        var connection = _adapterConnectionFactory.GetConnectionByShardKey(id);
        var commandDefinitions = new CommandDefinition(
            commandText: sqlAddCustomer,
            parameters: parameters,
            cancellationToken: token);
        await connection.ExecuteAsync(commandDefinitions);
    }

    private async Task<long> GetId(CancellationToken token)
    {
        const string sqlGetCustomerId = $@"
update {IdsTable}
set last_index = last_index + 1;

select  last_index
    from {IdsTable}
";
        
        await using var connectionForId = _connectionFactory.GetConnection(0);
        await connectionForId.OpenAsync(token);
        await using var transaction =
            await connectionForId.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);
        var commandDefinition = new CommandDefinition(
            commandText: sqlGetCustomerId,
            transaction: transaction,
            cancellationToken: token);
        var resultId = await connectionForId.QueryFirstAsync<long>(commandDefinition);
        await transaction.CommitAsync(token);
        _logger.LogDebug("Received customer id {id}", resultId);
        return resultId;
    }

    public Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<HasEmailResponseDto> HasEmail(HasEmailRequestDto request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private const string SelectFields = @"
customer_id as id, 
first_name as firstName, 
last_name as lastName, 
email, 
phone_number as phoneNumber";

    public const string InsertFields = @"
customer_id,
first_name,
last_name,
email,
phone_number,
date_creation
";

    private const string CustomerTable = "customer";
    private const string IdsTable = "is_source";
}