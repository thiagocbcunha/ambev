using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infra.Events;

public class MQTTSevices(ILogger<MQTTSevices> logger) : IEventBroker
{
    public Task PublishAsync<T>(T @event) where T : class
    {
        logger.LogInformation("Publishing event: {Event}", @event);
        return Task.CompletedTask;
    }
}