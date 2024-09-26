using Dapper;
using SportShopC_.Entities;
using System.Data.SqlClient;
using System.Data;
using SportShopC_.Repositories.Interfaces;

namespace SportShopC_.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction)
            : base(sqlConnection, dbtransaction, "Orders")
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            string sql = @"SELECT * FROM Orders WHERE UserId = @UserId";
            var result = await _sqlConnection.QueryAsync<Order>(sql,
                param: new { UserId = userId },
                transaction: _dbTransaction);
            return result;
        }
    }
}
