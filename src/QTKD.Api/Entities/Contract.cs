namespace QTKD.Api.Entities;

public class Contract
{
    public int Id { get; set; }
    public string ContractNo { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // Cho phép null: hợp đồng Draft có thể chưa đủ thông tin, bổ sung sau rồi mới gửi trình duyệt (CTR-02, tuần 06).
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public ContractStatus Status { get; set; } = ContractStatus.Draft;
}
