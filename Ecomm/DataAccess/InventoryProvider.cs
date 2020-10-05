using Dapper;
using Ecomm.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Ecomm.DataAccess
{
    public class InventoryProvider : IInventoryProvider
    {
        private readonly string _connectionString;

        public InventoryProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Inventory[] Get()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Inventory>(@"SELECT Id, Name, Quantity, ProductId FROM Inventory")
                .ToArray();
        }
    }
}
