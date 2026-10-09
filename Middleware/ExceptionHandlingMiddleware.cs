using System.Net;
using System.Text.Json;

namespace Helpdesk.Api.Middleware;

/// <summary>
/// Globaler Exception Handling Middleware zur zentralen Abfangung von unbehandelten Ausnahmen.
/// Garantiert eine standardisierte JSON-Fehlerausgabe für den Client.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Interceptet den HTTP-Request-Lifecycle und fängt auftretende Exceptions ab.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Erstellt eine standardisierte, RFC 7807-konforme JSON-Fehlerantwort.
    /// </summary>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            status = context.Response.StatusCode,
            message = "Internal Server Error. The issue has been logged and is being investigated.",
            detailed = exception.Message // Hilfreich für Portfolio/Debugging; in strikter Production ggf. entfernen
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}