using System.Net.Http.Headers;
using UserManagementAPI.Contracts;

namespace UserManagementAPI.Middleware;

public sealed class TokenAuthenticationMiddleware
{
    private static readonly PathString[] AnonymousPaths =
    [
        new("/openapi"),
        new("/swagger")
    ];

    private readonly RequestDelegate _next;
    private readonly ILogger<TokenAuthenticationMiddleware> _logger;
    private readonly string _expectedToken;

    public TokenAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<TokenAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _expectedToken = configuration["Authentication:Token"] ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldSkipAuthentication(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (string.IsNullOrWhiteSpace(_expectedToken))
        {
            _logger.LogError("Authentication token is not configured.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse("Authentication is not configured."));
            return;
        }

        if (!TryReadBearerToken(context.Request.Headers.Authorization, out var providedToken) ||
            !string.Equals(providedToken, _expectedToken, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                new ErrorResponse("Unauthorized. Provide a valid bearer token."));
            return;
        }

        context.Items["AuthenticatedToken"] = providedToken;
        await _next(context);
    }

    private static bool ShouldSkipAuthentication(PathString path)
    {
        return AnonymousPaths.Any(anonymousPath => path.StartsWithSegments(anonymousPath));
    }

    private static bool TryReadBearerToken(string? authorizationHeader, out string token)
    {
        token = string.Empty;

        if (string.IsNullOrWhiteSpace(authorizationHeader) ||
            !AuthenticationHeaderValue.TryParse(authorizationHeader, out var parsedHeader) ||
            !string.Equals(parsedHeader.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(parsedHeader.Parameter))
        {
            return false;
        }

        token = parsedHeader.Parameter;
        return true;
    }
}
