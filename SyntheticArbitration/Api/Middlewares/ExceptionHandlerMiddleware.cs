using System.Net;
using System.Text.Json;

namespace Api.Middlewares;

public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleExceptionMessageAsync(context, ex);
        }
    }

    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = statusCode,
            ErrorMessage = exception.Message,
            ErrorType = exception.GetType().Name
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException _ => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}