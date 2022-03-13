using System.Net;
public interface IException
{
    HttpStatusCode Code { get; set; }
    string Message { get; }
    string Type { get; set; }
}

public class ExceptionResponse : IException
{
    public HttpStatusCode Code { get; set; }
    public string Message { get; }
    public string Type { get; set; }
    public ExceptionResponse(BaseException exception)
    {
        Code = exception.Code;
        Message = exception.Message;
        Type = exception.Type;
    }
}

public class BaseException : Exception, IException
{
    public HttpStatusCode Code { get; set; }
    public string Type { get; set; } = "BaseException";

    public BaseException(string Message, HttpStatusCode Code, string Type) : base(Message)
    {
        this.Code = Code;
        this.Type = Type;
    }

    public static BaseException InternalServerError()
    {
        return new BaseException("Internal Server Error", HttpStatusCode.InternalServerError, "InternalServerError");
    }
}

public class HeaderNotFoundException : BaseException
{
    public HeaderNotFoundException(int headerIndex) : base($"Header not found at index {headerIndex}", HttpStatusCode.NotFound, "HeaderNotFoundException") { }
}