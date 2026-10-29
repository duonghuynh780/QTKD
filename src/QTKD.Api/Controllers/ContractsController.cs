using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    // GET /api/contracts/{id} -> 200 hoặc 404 (NotFoundException do Service ném, middleware đổi thành 404)
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContractResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _contractService.GetByIdAsync(id));
    }
}
