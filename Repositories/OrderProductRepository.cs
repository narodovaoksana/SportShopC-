using Dapper;
using SportShopC_.Entities;
using System.Data.SqlClient;
using System.Data;
using SportShopC_.Repositories.Interfaces;

namespace SportShopC_.Repositories
{
    public class OrderProductRepository : GenericRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction)
            : base(sqlConnection, dbtransaction, "OrderProducts")
        {
        }

        public async Task<IEnumerable<OrderProduct>> GetProductsByOrderIdAsync(int orderId)
        {
            string sql = @"SELECT * FROM OrderProducts WHERE OrderId = @OrderId";
            var result = await _sqlConnection.QueryAsync<OrderProduct>(sql,
                param: new { OrderId = orderId },
                transaction: _dbTransaction);
            return result;
        }
    }
}
