namespace QTKD.Api.Entities;

public enum CustomerStatus
{
    Active,
    Inactive,
}

/// <summary>
/// Trạng thái hợp đồng. Danh sách này là GIẢ ĐỊNH để thực hành, cần đối chiếu với
/// sơ đồ trạng thái hợp đồng ở Phase 1 (CTR-01 nhắc Draft/Revision Requested, CTR-03 nhắc Approved).
/// </summary>
public enum ContractStatus
{
    Draft,
    PendingApproval,
    RevisionRequested,
    Approved,
}
