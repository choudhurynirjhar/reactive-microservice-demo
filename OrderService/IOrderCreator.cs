using System.Threading.Tasks;

namespace OrderService
{
    public interface IOrderCreator
    {
        Task<int> Create(OrderDetail orderDetail);
    }
}