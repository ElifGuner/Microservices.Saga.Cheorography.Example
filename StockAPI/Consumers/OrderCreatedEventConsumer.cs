using MassTransit;
using Shared.Events;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            await Console.Out.WriteLineAsync(context.Message.OrderId + " - " + context.Message.BuyerId);
        }
    }
}
