using DropboxLike.Domain.Data;

namespace DropboxLike.Domain.Middlewares;

public class TokenvalidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApplicationDbContext _applicationDbContext;

    public TokenvalidationMiddleware(RequestDelegate next, ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        
        var user = _applicationDbContext.AppUsers.FirstOrDefault(t => t.Id == userId);

        if (user == null || user.Token != token)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        await _next(context);
    }
}