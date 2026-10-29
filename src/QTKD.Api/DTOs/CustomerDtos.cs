using QTKD.Api.Entities;

namespace QTKD.Api.DTOs;

public class CustomerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public static CustomerResponseDto FromEntity(Customer c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Status = c.Status.ToString(),
    };
}
