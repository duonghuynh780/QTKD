# Sơ đồ thành phần: QTKD (tuần 05)

## 1. Các thành phần chính của solution

```mermaid
flowchart LR
    Client["Client<br/>(Swagger / Postman)"] --> MW["ExceptionHandlingMiddleware"]
    MW --> Ctl["Controllers<br/>CustomersController<br/>ContractsController"]
    Ctl --> Svc["Services<br/>CustomerService<br/>ContractService"]
    Svc --> Repo["Repositories<br/>CustomerRepository<br/>ContractRepository"]
    Repo --> Db["QtkdDbContext<br/>(EF Core)"]
    Db --> Pg[("PostgreSQL<br/>qtkddb")]
```

| Thành phần | Vai trò |
|---|---|
| Controllers (API) | Nhận HTTP request, gọi Service, trả response/status code |
| Services (nghiệp vụ) | Chứa quy tắc nghiệp vụ; tuần 06 thêm CTR-01..03 và gửi hợp đồng trình duyệt |
| Repositories + Data (truy cập dữ liệu) | Đọc/ghi PostgreSQL qua EF Core |
| Entities / DTOs | Entity ánh xạ bảng; DTO là dữ liệu trả cho client |
| Middleware / Exceptions | Xử lý lỗi thống nhất (404, 500) |

## 2. Mô hình dữ liệu (ERD)

```mermaid
erDiagram
    CUSTOMER ||--o{ CONTRACT : "có"
    PRICELIST ||--|{ PRICEITEM : "gồm"

    CUSTOMER {
        int Id PK
        string Name
        string Status
    }
    CONTRACT {
        int Id PK
        string ContractNo UK
        int CustomerId FK
        date StartDate
        date EndDate
        string Status
    }
    PRICELIST {
        int Id PK
        string Name
        date EffectiveDate
    }
    PRICEITEM {
        int PriceListId PK, FK
        string ServiceCode PK
        decimal UnitPrice
    }
```

Ghi chú: các trường là gợi ý của đề thực hành, cần đối chiếu với mô hình dữ liệu Phase 1 (xem README mục 7).
