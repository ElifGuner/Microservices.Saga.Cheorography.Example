using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Models.Entities;
using OrderAPI.Models.ViewModels;
using Shared.Events;
using Shared.Messages;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        //readonly IPublishEndpoint _publishEndpoint;
        //public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndpoint)
        //{
        //    _context = context;
        //    _publishEndpoint = publishEndpoint;
        //}

        public OrdersController(OrderAPIDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
        {
            Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = createOrder.BuyerId,
                CreatedDate = DateTime.Now,
                OrderStatu = Models.Enums.OrderStatus.Suspend
            };

            order.OrderItems = createOrder.OrderItems.Select(oi => new OrderItem {
                ProductId = oi.ProductId,
                Count = oi.Count,
                Price = oi.Price
            }).ToList();

            order.TotalPrice = createOrder.OrderItems.Sum(oi => oi.Count * oi.Price);

            await  _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //OrderCreatedEvent orderCreatedEvent = new()
            //{
            //    BuyerId = order.BuyerId,
            //    OrderId = order.OrderId,
            //    OrderItems = order.OrderItems.Select(oi => new OrderItemMessage {
            //        Count = oi.Count,
            //        ProductId = oi.ProductId
            //    }).ToList()
            //};
            //await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok();
        }
    }
}
