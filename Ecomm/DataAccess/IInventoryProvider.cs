using Ecomm.Models;

namespace Ecomm.DataAccess
{
    public interface IInventoryProvider
    {
        Inventory[] Get();
    }
}