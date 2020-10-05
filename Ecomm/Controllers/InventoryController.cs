using System.Collections.Generic;
using Ecomm.DataAccess;
using Ecomm.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryProvider _inventoryProvider;

        public InventoryController(IInventoryProvider inventoryProvider)
        {
            _inventoryProvider = inventoryProvider;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return _inventoryProvider.Get();
        }

        // GET api/<InventoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InventoryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InventoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
