namespace QTKD.Api.Entities;

/// <summary>
/// Một dòng giá của bảng giá. Khóa chính là cặp (PriceListId, ServiceCode):
/// một bảng giá không có hai dòng cùng mã dịch vụ.
/// </summary>
public class PriceItem
{
    public int PriceListId { get; set; }
    public PriceList PriceList { get; set; } = null!;

    public string ServiceCode { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
