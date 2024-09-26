using Dapper;
using SportShopC_.Entities;
using System.Data.SqlClient;
using System.Data;

using SportShopC_.Repositories.Interfaces;

namespace SportShopC_.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction)
            : base(sqlConnection, dbtransaction, "Users")
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            string sql = @"SELECT * FROM Users WHERE Email = @Email";
            var result = await _sqlConnection.QuerySingleOrDefaultAsync<User>(sql,
                param: new { Email = email },
                transaction: _dbTransaction);
            return result;
        }
    }
}
