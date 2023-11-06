using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Messages;
using StockAPI.Models;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        StockAPIDbContext _dbContext;
        ISendEndpointProvider _sendEndpointProvider;
        IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(StockAPIDbContext dbContext, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
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
                    Stock? stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);
                    if (stock != null)
                         stock.Count -= orderItem.Count;
                }
                await _dbContext.SaveChangesAsync();

                //Payment

                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice
                };

                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue : {RabbitMQSettings.Payment_StockReservedEventQueue}"));
                await sendEndpoint.Send(stockReservedEvent);

                await Console.Out.WriteLineAsync("Stock işlemleri başarılı...");
            }
            else 
            {
                //Siparişin geçersiz olduğuna dair işlemler
                StockNotReservedEvent stockNotReservedEvent = new() 
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "..."
                };
                _publishEndpoint.Publish(stockNotReservedEvent);

                await Console.Out.WriteLineAsync("Stock işlemleri başarısız...");

            }
            //return Task.CompletedTask;
        }
    }
}
