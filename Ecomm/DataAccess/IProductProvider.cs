using Ecomm.Models;

namespace Ecomm.DataAccess
{
    public interface IProductProvider
    {
        Product[] Get();
    }
}