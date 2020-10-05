using Ecomm.Models;
using System.Threading.Tasks;

namespace Ecomm.DataAccess
{
    public interface IOrderDetailsProvider
    {
        Task<OrderDetail[]> Get();
    }
}