using System.Net;
using FluentValidation;
using MESK.ResponseEntity;
using MeskChatApplication.Application.Exceptions;
using MeskChatApplication.Domain.Dtos;
using Newtonsoft.Json;

namespace MeskChatApplication.WebApi.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var result = ResponseEntity<EmptyResponse>.Failure();
        result.WithMessage(ex.Message);
        
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            ApplicationLogicBaseException e => e.StatusCode,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        
        result.WithStatusCode(statusCode);

        if (ex.GetType() == typeof(ValidationException))
        {
            var validationErrors = ((ValidationException)ex).Errors
                .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);
            result.WithValidationErrors(validationErrors);
        }
        
        context.Response.StatusCode = result.StatusCode;
        
        return context.Response.WriteAsync(result.ToString());
    }
}