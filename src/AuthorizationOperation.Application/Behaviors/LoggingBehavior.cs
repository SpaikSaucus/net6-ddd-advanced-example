using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // uncomment for more details.
            //this.logger.LogInformation("Executing command {CommandName} with request {@Command}", request.GetType().FullName, request);
            this.logger.LogInformation("Executing command {CommandName}", request.GetType().FullName);

            TResponse response = await next();

            // uncomment for more details.
            //this.logger.LogInformation("Executed command {CommandName} with response {@Response}", request.GetType().FullName, response);
            this.logger.LogInformation("Executed command {CommandName}", request.GetType().FullName);

            return response;
        }
    }
}
