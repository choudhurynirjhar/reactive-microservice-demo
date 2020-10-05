using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService
{
    public class ReportDataCollector : IHostedService
    {
        private const int DEFAULT_QUANTITY = 100;
        private readonly ISubscriber subscriber;
        private readonly IMemoryReportStorage memoryReportStorage;

        public ReportDataCollector(ISubscriber subscriber, IMemoryReportStorage memoryReportStorage)
        {
            this.subscriber = subscriber;
            this.memoryReportStorage = memoryReportStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }

        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            if (message.Contains("Product"))
            {
                var product = JsonConvert.DeserializeObject<Product>(message);
                if (memoryReportStorage.Get().Any(r => r.ProductName == product.ProductName))
                {
                    return true;
                }
                else
                {
                    memoryReportStorage.Add(new Report
                    {
                        ProductName = product.ProductName,
                        Count = DEFAULT_QUANTITY
                    });
                }
            }
            else
            {
                var order = JsonConvert.DeserializeObject<Order>(message);
                if(memoryReportStorage.Get().Any(r => r.ProductName == order.Name))
                {
                    memoryReportStorage.Get().First(r => r.ProductName == order.Name).Count -= order.Quantity;
                }
                else
                {
                    memoryReportStorage.Add(new Report
                    {
                        ProductName = order.Name,
                        Count = DEFAULT_QUANTITY - order.Quantity
                    });
                }
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
