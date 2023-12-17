using System.Net;
using Web_Api.Models.Contracts;

namespace Web_Api.Middleware;
/// <summary>
/// Middlwere для обработки глабальных исключении.
/// </summary>
public sealed class ExceptionHandlingMiddlwere : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddlwere> _logger;

    public ExceptionHandlingMiddlwere(ILogger<ExceptionHandlingMiddlwere> logger) =>
        _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, ex.Message);
            await HandleExeptionAsync(context, ex.Message, HttpStatusCode.InternalServerError);
        }
    }
    /// <summary>
    /// Асинхронный метод обработки исключении. 
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="exMassage">Сообщение ошибки</param>
    /// <param name="httpStatusCode">Статус код ошибки</param>
    /// <returns></returns>
    private async Task HandleExeptionAsync(HttpContext context, string exMassage, HttpStatusCode httpStatusCode)
    {
        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        ErrorDto errorDto = new()
        {
            Message = exMassage,
            StatusCode = (int)httpStatusCode,
        };

        await response.WriteAsJsonAsync(errorDto);
    }
}