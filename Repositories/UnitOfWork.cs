using System.Data;
using SportShopC_.Repositories.Interfaces;
namespace SportShopC_.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IUserRepository _userRepository { get; }
        public IOrderRepository _orderRepository { get; }
        public IOrderProductRepository _orderProductRepository { get; }

        private readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IUserRepository userRepository,
                          IOrderRepository orderRepository,
                          IOrderProductRepository orderProductRepository,
                          IDbTransaction dbTransaction)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _dbTransaction = dbTransaction;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
