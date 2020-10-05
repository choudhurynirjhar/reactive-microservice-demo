using System;

namespace Ecomm.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
