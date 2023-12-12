﻿using System.Net;
using Web_Api.Models.Contracts;

namespace Web_Api.Middleware;

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