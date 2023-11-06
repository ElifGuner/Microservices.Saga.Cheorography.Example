using MassTransit;
using Shared.Events;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        StockAPIDbContext _dbContext;

        public OrderCreatedEventConsumer(StockAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            // await Console.Out.WriteLineAsync(context.Message.OrderId + " - " + context.Message.BuyerId);
            List<bool> stockResult = new();
           foreach (OrderItemMessage orderItem in context.Message.OrderItems) 
           {
                //await _dbContext.Stocks.FirstOrDefault(s => s.ProductId == orderItem.ProductId);
                stockResult.Add(_dbContext.Stocks.Any(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count));
               //orderItem.ProductId
                //await orderItem.Consume(context);
           }

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {

                }
           return Task.CompletedTask;
        }
    }
}
