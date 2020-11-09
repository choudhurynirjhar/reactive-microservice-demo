using Ecomm.DataAccess;
using Ecomm.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm
{
    public class OrderCreatedListener : IHostedService
    {
        private readonly IPublisher publisher;
        private readonly ISubscriber subscriber;
        private readonly IInventoryUpdator inventoryUpdator;

        public OrderCreatedListener(IPublisher publisher, ISubscriber subscriber, IInventoryUpdator inventoryUpdator)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.inventoryUpdator = inventoryUpdator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<OrderRequest>(message);
            try
            {
                inventoryUpdator.Update(response.ProductId, response.Quantity).GetAwaiter().GetResult();
                publisher.Publish(JsonConvert.SerializeObject(
                    new InventoryResponse { OrderId = response.OrderId, IsSuccess = true }
                    ), "inventory.response", null);
            }
            catch (Exception)
            {
                publisher.Publish(JsonConvert.SerializeObject(
                    new InventoryResponse { OrderId = response.OrderId, IsSuccess = false }
                    ), "inventory.response", null);
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    

    
}
