using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDetailsProvider orderDetailsProvider;
        private readonly IPublisher publisher;

        public OrderController(IOrderDetailsProvider orderDetailsProvider, IPublisher publisher)
        {
            this.orderDetailsProvider = orderDetailsProvider;
            this.publisher = publisher;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<OrderDetail> Get()
        {
            return orderDetailsProvider.Get();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] OrderDetail orderDetail)
        {
            // Order insert into database
            publisher.Publish(JsonConvert.SerializeObject(orderDetail), "report.order", null);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
