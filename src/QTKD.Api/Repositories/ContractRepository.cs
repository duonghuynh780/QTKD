using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Entities;

namespace QTKD.Api.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly QtkdDbContext _db;

    public ContractRepository(QtkdDbContext db)
    {
        _db = db;
    }

    // Include: nạp luôn Customer để trả về tên khách hàng.
    // Tuần 06 (cập nhật/gửi trình duyệt) sẽ cần bản có tracking để sửa và lưu Contract.
    public Task<Contract?> GetByIdAsync(int id)
    {
        return _db.Contracts
            .AsNoTracking()
            .Include(c => c.Customer)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
