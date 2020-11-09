using System.Threading.Tasks;

namespace OrderService
{
    public interface IOrderDeletor
    {
        Task Delete(int orderId);
    }
}