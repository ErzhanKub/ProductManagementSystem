﻿using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middleware;
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

            _logger.LogError(ex, "Exception occurred: {message}", ex.Message);
            await HandleExeptionAsync(context, ex.Message);
        }
    }
    /// <summary>
    /// Асинхронный метод обработки исключении. 
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="exMassage">Сообщение ошибки</param>
    /// <param name="httpStatusCode">Статус код ошибки</param>
    /// <returns></returns>
    private async Task HandleExeptionAsync(HttpContext context, string exMassage)
    {
        HttpResponse response = context.Response;
        response.ContentType = "application/problem+json";
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exMassage,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await response.WriteAsJsonAsync(problemDetails);
    }
}