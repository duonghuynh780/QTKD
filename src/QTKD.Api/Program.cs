using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Middleware;
using QTKD.Api.Repositories;
using QTKD.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Connection string lấy từ configuration. Mật khẩu KHÔNG nằm trong source/Git:
// appsettings.json chỉ có host/database/user, mật khẩu nằm trong user-secrets (xem README).
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing configuration: ConnectionStrings:Default");

// Scoped: mỗi HTTP request có một DbContext riêng.
builder.Services.AddDbContext<QtkdDbContext>(options => options.UseNpgsql(connectionString));

// Controller → Service → Repository → DbContext (đều Scoped vì phụ thuộc DbContext).
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Đặt đầu pipeline để bắt exception của mọi thứ phía sau.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
