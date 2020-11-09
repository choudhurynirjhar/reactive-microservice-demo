using Ecomm.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService
{
    public class InventoryResponseListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IOrderDeletor orderDeletor;

        public InventoryResponseListener(ISubscriber subscriber, IOrderDeletor orderDeletor)
        {
            this.subscriber = subscriber;
            this.orderDeletor = orderDeletor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<InventoryResponse>(message);
            if (!response.IsSuccess)
            {
                orderDeletor.Delete(response.OrderId).GetAwaiter().GetResult();
            }
            return true;
        } 

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
