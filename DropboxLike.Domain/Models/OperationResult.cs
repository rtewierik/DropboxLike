namespace DropboxLike.Domain.Models;

public class OperationResult<T>
{
    public bool Successful { get; protected set; }
    public T? Result { get; protected set; }
    public string? FailureMessage { get; protected set; }
    public Exception? Exception { get; protected set; }
    
    protected OperationResult(T result)
    {
        Successful = true;
        Result = result;
        FailureMessage = null;
        Exception = null;
    }
    
    protected OperationResult(string message)
    {
        Successful = false;
        FailureMessage = message;
        Exception = null;
        Result = default;
    }
    
    protected OperationResult(Exception exception, string? message = null)
    {
        Successful = false;
        Exception = exception;
        FailureMessage = message ?? exception.Message;
        Result = default;
    }
    
    public static OperationResult<T> SuccessResult(T result)
    {
        return new OperationResult<T>(result);
    }
    
    public static OperationResult<T> FailureResult(string message)
    {
        return new OperationResult<T>(message);
    }
    
    public static OperationResult<T> ExceptionResult(Exception exception, string? message = null)
    {
        return new OperationResult<T>(exception, message);
    }
    
    public bool IsException()
    {
        return Exception != null;
    }
}