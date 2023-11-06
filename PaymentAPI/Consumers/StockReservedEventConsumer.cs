using MassTransit;
using MassTransit.Transports;
using Shared.Events;
using System.Collections.Concurrent;

namespace PaymentAPI.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //ödeme işlemleri
            if (true)
            {
                //ödeme işlemleri başarılıysa

                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };

                _publishEndpoint.Publish(paymentCompletedEvent);

                Console.WriteLine("Ödeme başarılı...");
            }
            else
            {
                //ödeme işlemleri başarısızsa
                PaymentFailedEvent paymentFailedEvent = new() 
                {
                    OrderId = context.Message.OrderId,
                    Message ="Bakiye yetersiz..."
                };

                _publishEndpoint.Publish(paymentFailedEvent);

                Console.WriteLine("Ödeme başarısız...");
            }

            return Task.CompletedTask;
        }
    }
}
