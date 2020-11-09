using Ecomm.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plain.RabbitMQ;
using RabbitMQ.Client;

namespace Ecomm
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
            services.AddControllersWithViews();

            var connectionString = Configuration["ConnectionString"];

            services.AddSingleton<IOrderDetailsProvider, OrderDetailsProvider>();
            services.AddSingleton<IInventoryProvider>(new InventoryProvider(connectionString));
            services.AddSingleton<IProductProvider>(new ProductProvider(connectionString));
            services.AddSingleton<IInventoryUpdator>(new InventoryUpdator(connectionString));

            services.AddHttpClient("order", config =>
                config.BaseAddress = new System.Uri("https://localhost:5001/"));

            services.AddSingleton<IConnectionProvider>(new ConnectionProvider("amqp://guest:guest@localhost:5672"));
            services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
                    "inventory_exchange",
                    ExchangeType.Topic));
            services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
                "order_exchange",
                "order_response",
                "order.created",
                ExchangeType.Topic));

            services.AddHostedService<OrderCreatedListener>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ecomm service",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecomm Service");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
