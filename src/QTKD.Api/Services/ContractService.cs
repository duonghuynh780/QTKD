using QTKD.Api.DTOs;
using QTKD.Api.Exceptions;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

/// <summary>
/// Tuần 05 chỉ có thao tác đọc. Tuần 06 sẽ thêm tạo/sửa/gửi trình duyệt và các quy tắc CTR-01..03 tại đây.
/// </summary>
public class ContractService : IContractService
{
    private readonly IContractRepository _contracts;

    public ContractService(IContractRepository contracts)
    {
        _contracts = contracts;
    }

    public async Task<ContractResponseDto> GetByIdAsync(int id)
    {
        var contract = await _contracts.GetByIdAsync(id)
            ?? throw new NotFoundException($"Contract with Id = {id} was not found.");
        return ContractResponseDto.FromEntity(contract);
    }
}
