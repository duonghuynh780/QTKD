# QTKD: hệ thống Quản trị kinh doanh (Phase 2, tuần 05)

ASP.NET Core Web API + Entity Framework Core + PostgreSQL. Tuần 05 chuẩn bị nền tảng: entity, migration,
dữ liệu mẫu và hai endpoint đọc dữ liệu. Tuần 06 và 07 tiếp tục trên cùng repository này.

## 1. Yêu cầu môi trường

- .NET SDK 10 (`dotnet --version`)
- PostgreSQL đang chạy (kèm pgAdmin để xem dữ liệu)
- Git
- Công cụ EF Core CLI: `dotnet tool install --global dotnet-ef`

## 2. Cấu hình database (không đưa mật khẩu lên Git)

`src/QTKD.Api/appsettings.json` chỉ chứa host/database/user, **không có mật khẩu**. Mật khẩu để trong
user-secrets, lưu ngoài thư mục source nên không bị commit. Chạy từ thư mục gốc của repository:

```bash
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=qtkddb;Username=postgres;Password=MAT_KHAU_CUA_BAN" --project src/QTKD.Api
```

Người chạy trên máy khác tự thay `MAT_KHAU_CUA_BAN` bằng mật khẩu PostgreSQL của họ.

## 3. Tạo database bằng migration

```bash
dotnet ef database update --project src/QTKD.Api
```

Lệnh này tự tạo database `qtkddb` nếu chưa có, tạo 4 bảng `Customers`, `Contracts`, `PriceLists`, `PriceItems`
và nạp dữ liệu mẫu. Thư mục `src/QTKD.Api/Migrations` (do `dotnet ef migrations add InitialCreate` tạo ra) được
commit cùng repository, nên máy khác chỉ cần chạy `database update`.

## 4. Chạy API

```bash
dotnet run --project src/QTKD.Api
```

Swagger: `http://localhost:5090/swagger`

## 5. Thử hai endpoint

| Endpoint | Kết quả mong đợi |
|---|---|
| `GET /api/customers` | 200, 2 khách hàng: "Cong ty Minh Phat" (Id 1), "Cong ty Hai Au" (Id 2) |
| `GET /api/contracts/1` | 200, hợp đồng `HD-2026-018` của Minh Phat, trạng thái `Draft` |
| `GET /api/contracts/99` | 404, `Contract with Id = 99 was not found.` |

Đối chiếu với database: mở bảng `Customers` và `Contracts` trong pgAdmin, dữ liệu phải khớp với JSON trả về.

## 6. Cấu trúc repository

```
QTKD
├── src/QTKD.Api
│   ├── Controllers/    nhận request, gọi Service, trả response (không nghiệp vụ, không DbContext)
│   ├── Services/       xử lý nghiệp vụ (tuần 06: quy tắc CTR-01..03, gửi hợp đồng trình duyệt)
│   ├── Repositories/   truy cập dữ liệu bằng EF Core
│   ├── Data/           QtkdDbContext: cấu hình bảng, quan hệ, dữ liệu mẫu
│   ├── Entities/       Customer, Contract, PriceList, PriceItem
│   ├── DTOs/           dữ liệu trả về cho client
│   ├── Exceptions/     NotFoundException (404)...
│   ├── Middleware/     ExceptionHandlingMiddleware
│   └── Migrations/     do EF Core sinh ra
├── tests/              project xUnit (tuần 07)
├── docs/               sơ đồ thành phần, ERD, tài liệu API, minh chứng
└── README.md
```

Luồng request: Client → `ExceptionHandlingMiddleware` → Controller → Service → Repository → `QtkdDbContext` → PostgreSQL.

## 7. Giả định cần đối chiếu với Phase 1

Các trường dưới đây là gợi ý của đề thực hành, **chưa phải đặc tả đã xác nhận**; cần đối chiếu với mô hình dữ liệu và
sơ đồ trạng thái ở Phase 1 và cập nhật (kèm lý do) nếu khác:

- `Customer`: Id, Name, Status (`Active`/`Inactive`).
- `Contract`: Id, ContractNo, CustomerId, StartDate, EndDate, Status. `StartDate` và `EndDate` cho phép null để hợp đồng Draft
  có thể chưa đủ thông tin (phục vụ quy tắc CTR-02 ở tuần 06).
- Trạng thái hợp đồng: `Draft`, `PendingApproval`, `RevisionRequested`, `Approved`.
- `PriceItem` có khóa chính là cặp (`PriceListId`, `ServiceCode`).
- Dữ liệu mẫu dùng tên không dấu để tránh lỗi mã hóa tùy cấu hình PostgreSQL.

## 8. Phạm vi tuần 05

Có: kết nối PostgreSQL, entity, migration, dữ liệu mẫu, `GET /api/customers`, `GET /api/contracts/{id}`.
Chưa có (để tuần 06): tạo/sửa Customer và Contract, bảng giá API, gửi hợp đồng trình duyệt, kiểm tra quy tắc nghiệp vụ.
