using System.Net;
using System.Text;

namespace AddressPrishtina.Middleware;

public class ExceptionHandler : IMiddleware
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception Occured");
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(e.Message));
        }
    }
}