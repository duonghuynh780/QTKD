using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public class ContractResponseDto
{
    public int Id { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;

    public static ContractResponseDto FromEntity(Contract c) => new()
    {
        Id = c.Id,
        ContractNo = c.ContractNo,
        CustomerId = c.CustomerId,
        CustomerName = c.Customer.Name,
        StartDate = c.StartDate,
        EndDate = c.EndDate,
        Status = c.Status.ToString(),
    };
}
