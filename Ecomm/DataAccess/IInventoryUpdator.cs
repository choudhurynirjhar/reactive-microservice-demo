using System.Threading.Tasks;

namespace Ecomm.DataAccess
{
    public interface IInventoryUpdator
    {
        Task Update(int productId, int quantity);
    }
}