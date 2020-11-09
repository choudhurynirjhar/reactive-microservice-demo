using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService
{
    public class OrderCreator : IOrderCreator
    {
        private readonly string connectionString;
        private readonly ILogger<OrderCreator> logger;

        public OrderCreator(string connectionString, ILogger<OrderCreator> logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public async Task<int> Create(OrderDetail orderDetail)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var id = await connection.QuerySingleAsync<int>("CREATE_ORDER", new { userId = 1, userName = orderDetail.User }, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                await connection.ExecuteAsync("CREATE_ORDER_DETAILS",
                    new { orderId = id, productId = orderDetail.ProductId, quantity = orderDetail.Quantity, productName = orderDetail.Name }, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                transaction.Commit();
                return id;
            }
            catch(Exception exc)
            {
                logger.LogError($"Error: {exc}");
                transaction.Rollback();
                return -1;
            }
        }
    }
}
