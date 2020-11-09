using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Order service",
                    Version = "v1"
                });
            });
            var connectionString = Configuration["ConnectionString"];
            services.AddSingleton<IOrderDetailsProvider>(new OrderDetailsProvider(connectionString));
            services.AddSingleton<IOrderCreator>(x => new OrderCreator(connectionString, x.GetService<ILogger<OrderCreator>>()));
            services.AddSingleton<IOrderDeletor>(new OrderDeletor(connectionString));

            services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
            services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
                    "order_exchange",
                    ExchangeType.Topic));
            services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
                "inventory_exchange",
                "inventory_response",
                "inventory.response",
                ExchangeType.Topic));

            services.AddHostedService<InventoryResponseListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
