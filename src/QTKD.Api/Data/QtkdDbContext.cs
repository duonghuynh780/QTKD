using Microsoft.EntityFrameworkCore;
using QTKD.Api.Entities;

namespace QTKD.Api.Data;

/// <summary>
/// DbContext của hệ thống QTKD: ánh xạ entity sang bảng PostgreSQL,
/// khai báo quan hệ (Contract–Customer, PriceList–PriceItem) và dữ liệu mẫu.
/// </summary>
public class QtkdDbContext : DbContext
{
    public QtkdDbContext(DbContextOptions<QtkdDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<PriceList> PriceLists => Set<PriceList>();
    public DbSet<PriceItem> PriceItems => Set<PriceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(200);

            // Lưu trạng thái dạng chữ ("Active") thay vì số, để đọc trực tiếp trong pgAdmin.
            e.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);

            e.HasData(
                new Customer { Id = 1, Name = "Cong ty Minh Phat", Status = CustomerStatus.Active },
                new Customer { Id = 2, Name = "Cong ty Hai Au", Status = CustomerStatus.Active });
        });

        modelBuilder.Entity<Contract>(e =>
        {
            e.Property(c => c.ContractNo).IsRequired().HasMaxLength(50);
            e.HasIndex(c => c.ContractNo).IsUnique(); // không trùng số hợp đồng ở tầng database
            e.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);

            // Restrict: không cho xóa Customer đang có hợp đồng.
            e.HasOne(c => c.Customer)
                .WithMany(x => x.Contracts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasData(new Contract
            {
                Id = 1,
                ContractNo = "HD-2026-018",
                CustomerId = 1,
                StartDate = new DateOnly(2026, 11, 1),
                EndDate = new DateOnly(2027, 10, 31),
                Status = ContractStatus.Draft,
            });
        });

        modelBuilder.Entity<PriceList>(e =>
        {
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);

            e.HasData(new PriceList
            {
                Id = 1,
                Name = "Bang gia 2026",
                EffectiveDate = new DateOnly(2026, 11, 1),
            });
        });

        modelBuilder.Entity<PriceItem>(e =>
        {
            e.HasKey(i => new { i.PriceListId, i.ServiceCode });
            e.Property(i => i.ServiceCode).IsRequired().HasMaxLength(50);
            e.Property(i => i.UnitPrice).HasPrecision(18, 2);

            // Cascade: xóa bảng giá thì xóa luôn các dòng giá của nó.
            e.HasOne(i => i.PriceList)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasData(
                new PriceItem { PriceListId = 1, ServiceCode = "LOAD-20", UnitPrice = 450000m },
                new PriceItem { PriceListId = 1, ServiceCode = "STORAGE-DAY", UnitPrice = 120000m });
        });
    }
}
