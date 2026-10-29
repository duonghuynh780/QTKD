using QTKD.Api.DTOs;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customers;

    public CustomerService(ICustomerRepository customers)
    {
        _customers = customers;
    }

    public async Task<List<CustomerResponseDto>> GetAllAsync()
    {
        var list = await _customers.GetAllAsync();
        return list.Select(CustomerResponseDto.FromEntity).ToList();
    }
}
