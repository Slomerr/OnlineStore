using System.Data;
using Dapper;
using giks.OnlineStore.CustomerService.Application.Customers;
using giks.OnlineStore.CustomerService.Application.Customers.AddCustomer;
using giks.OnlineStore.CustomerService.Application.Customers.GetCustomerById;
using giks.OnlineStore.CustomerService.Application.Customers.HasEmail;
using giks.OnlineStore.CustomerService.Application.Customers.UpdateCustomer;
using giks.OnlineStore.CustomerService.Infrastructure.Dal.Connections;
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
        await using var connection = _adapterConnectionFactory.GetConnectionByShardKey(request.CustomerId);
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
        var mainConnection = _connectionFactory.GetConnection(0);
        await mainConnection.OpenAsync(token);
        await using var transaction = await mainConnection.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

        var connectionDto = new ConnectionDto(mainConnection, transaction);
        
        var id = await GetId(connectionDto, token);
        
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

        var emailDto = new EmailDto(request.Customer.Email, id);

        await InsertEmail(emailDto, connectionDto, token);
        await transaction.CommitAsync(token);
    }

    private async Task<long> GetId(ConnectionDto connectionDto, CancellationToken token)
    {
        const string sqlGetCustomerId = $@"
update {IdsTable}
set last_index = last_index + 1;

select  last_index
    from {IdsTable}
";
        
        var commandDefinition = new CommandDefinition(
            commandText: sqlGetCustomerId,
            transaction: connectionDto.Transaction,
            cancellationToken: token);
        var resultId = await connectionDto.Connection.QueryFirstAsync<long>(commandDefinition);
        _logger.LogDebug("Received customer id {id}", resultId);
        return resultId;
    }
    
    private async Task InsertEmail(EmailDto emailDto, ConnectionDto connectionDto, CancellationToken token)
    {
        const string sqlRequest = $@"
insert into {EmailIndexTable} (email, customer_id)
values (:email, :id)
;
";

        var parameters = new DynamicParameters();
        parameters.Add("email", emailDto.Email);
        parameters.Add("id", emailDto.Id);

        var commandDefinition = new CommandDefinition(
            commandText: sqlRequest,
            parameters: parameters,
            transaction: connectionDto.Transaction,
            cancellationToken: token);
        await connectionDto.Connection.ExecuteAsync(commandDefinition);
    }

    private async Task UpdateEmail(EmailDto emailDto, ConnectionDto connectionDto, CancellationToken token)
    {
        const string sqlRequest = $@"
update {EmailIndexTable}
    set email = :email
where customer_id = :id
and email != :email
;
";

        var parameters = new DynamicParameters();
        parameters.Add("email", emailDto.Email);
        parameters.Add("id", emailDto.Id);

        var commandDefinition = new CommandDefinition(
            commandText: sqlRequest,
            parameters: parameters,
            transaction: connectionDto.Transaction,
            cancellationToken: token);
        await connectionDto.Connection.ExecuteAsync(commandDefinition);
    }

    public async Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto request, CancellationToken token)
    {
        const string sqlRequest = $@"
update {CustomerTable}
set first_name = :first_name,
    last_name = :last_name,
    email = :email,
    phone_number = :phone_number
where customer_id = :id;
";
        
        var parameters = new DynamicParameters();
        parameters.Add("first_name", request.Customer.FirstName);
        parameters.Add("last_name", request.Customer.LastName);
        parameters.Add("email", request.Customer.Email);
        parameters.Add("phone_number", request.Customer.PhoneNumber);
        parameters.Add("id", request.Customer.Id);

        await using var connection = _adapterConnectionFactory.GetConnectionByShardKey(request.Customer.Id);
        await connection.OpenAsync(token);
        await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);
        
        var commandDefinition = new CommandDefinition(
            commandText: sqlRequest,
            transaction: transaction,
            parameters: parameters,
            cancellationToken: token);

        await connection.ExecuteAsync(commandDefinition);

        await using var baseConnection = _connectionFactory.GetConnection(0);
        await baseConnection.OpenAsync(token);
        await using var baseTransaction =
            await baseConnection.BeginTransactionAsync(IsolationLevel.RepeatableRead, token);

        var connectionDto = new ConnectionDto(baseConnection, baseTransaction);
        var emailDto = new EmailDto(request.Customer.Email, request.Customer.Id);

        await UpdateEmail(emailDto, connectionDto, token);
        
        //TODO: need solve this problem zone
        await transaction.CommitAsync(token);
        await baseTransaction.CommitAsync(token);
        
        return new UpdateCustomerResponseDto(true);
    }

    public async Task<HasEmailResponseDto> HasEmail(HasEmailRequestDto request, CancellationToken token)
    {
        const string sqlRequest = $@"
select customer_id
    from {EmailIndexTable}
where email = :email
limit 1
;
";

        var parameters = new DynamicParameters();
        parameters.Add("email",  request.Email);
        
        await using var baseConnection = _connectionFactory.GetConnection(0);
        var commandDefinition = new CommandDefinition(
            commandText: sqlRequest,
            parameters: parameters,
            cancellationToken: token);
        var result = await baseConnection.QuerySingleAsync<long?>(commandDefinition);
        return new HasEmailResponseDto(result == null);
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
    private const string EmailIndexTable = "email_global_t_idx";
}