using System;
using Project03_Shop.Middleware;

namespace Project03_Shop.Middleware;

public static class MiddlewareExtensions
{
  public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder) {
    return builder.UseMiddleware<RequestLoggingMiddleware>();
  }
}

