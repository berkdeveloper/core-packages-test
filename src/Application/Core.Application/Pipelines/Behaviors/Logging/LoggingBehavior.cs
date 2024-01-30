using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.Application.Pipelines.Behaviors.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        _logger.LogInformation($"Handling ({requestName})");

        TResponse response;
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation($"[START] {requestName};");

        try { response = await next(); }
        finally
        {
            _logger.LogInformation($"Handled ({responseName})");
            stopwatch.Stop();
            _logger.LogInformation($"[END] {requestName}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }

        return response;
    }
}
