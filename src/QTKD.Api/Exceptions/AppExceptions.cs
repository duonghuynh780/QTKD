namespace QTKD.Api.Exceptions;

/// <summary>
/// Lỗi nghiệp vụ kèm HTTP status code. Service ném, ExceptionHandlingMiddleware bắt ở một chỗ
/// và đổi thành response thống nhất. Tuần 06 sẽ thêm các lỗi như BusinessRuleException cho CTR-01..03.
/// </summary>
public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public string Title { get; }

    protected AppException(int statusCode, string title, string message) : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }
}

/// <summary>Không tìm thấy tài nguyên: 404.</summary>
public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(StatusCodes.Status404NotFound, "Not Found", message) { }
}
