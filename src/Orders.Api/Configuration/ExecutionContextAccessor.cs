using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Orders.Application.Configuration;

namespace Orders.Api.Configuration;

public class ExecutionContextAccessor : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid RequestId
    {
        get
        {
            if (IsAvailable &&
                _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(
                    x => x == CorrelationMiddleware.CorrelationHeaderKey
                ))
                return Guid.Parse(
                    _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]
                );
            throw new ApplicationException("Http context and Request-Id is not available");
        }
    }

    public bool IsAvailable => _httpContextAccessor.HttpContext != null;
}