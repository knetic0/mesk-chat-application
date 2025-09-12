using System.Security.Claims;
using System.Threading.RateLimiting;
using MESK.ResponseEntity;
using MeskChatApplication.Domain.Dtos;
using Microsoft.AspNetCore.RateLimiting;

namespace MeskChatApplication.WebApi.Extensions;

public static class RateLimiterExtension
{
    public static void UseImprovedRateLimiter(this IApplicationBuilder app)
    {
        app.UseRateLimiter(new RateLimiterOptions
        {
            GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                if (httpContext.Request.Path.StartsWithSegments("/chat"))
                    return RateLimitPartition.GetNoLimiter<string>("SignalR");
                
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userId, out var _))
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"user_{userId}",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 50,
                            Window = TimeSpan.FromMinutes(1)
                        });
                }
                
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: $"ip_{GetClientIpAddress(httpContext)}",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromSeconds(1)
                    });
            }),
            OnRejected = (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                var responseEntity = ResponseEntity<EmptyResponse>.Failure();
                responseEntity.WithMessage("Too many requests.");
                context.HttpContext.Response.WriteAsJsonAsync(responseEntity.ToString(), cancellationToken);
                return new ValueTask();
            }
        });
    }
    
    private static string GetClientIpAddress(HttpContext context)
    {
        var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(xForwardedFor))
        {
            var ips = xForwardedFor.Split(',');
            if (ips.Length > 0)
                return ips[0].Trim();
        }
    
        var xRealIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(xRealIp))
            return xRealIp;
    
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}