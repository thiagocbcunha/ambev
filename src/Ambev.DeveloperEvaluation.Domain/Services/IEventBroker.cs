namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IEventBroker
{
    Task PublishAsync<T>(T @event) where T : class;
}
