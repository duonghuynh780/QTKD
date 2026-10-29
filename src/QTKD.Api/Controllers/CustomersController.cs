using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

/// <summary>Controller chỉ nhận request, gọi Service, trả response; không chứa nghiệp vụ, không gọi DbContext.</summary>
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET /api/customers -> 200
    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _customerService.GetAllAsync());
    }
}
