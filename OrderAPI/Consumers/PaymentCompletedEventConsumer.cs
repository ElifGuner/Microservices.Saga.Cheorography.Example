using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.Models.Entities;
using Shared.Events;

namespace OrderAPI.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;
        public PaymentCompletedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }
        
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
           Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o=> o.OrderId == context.Message.OrderId);
           order.OrderStatu = Models.Enums.OrderStatus.Completed;
           await _orderAPIDbContext.SaveChangesAsync();

        }
    }
}
