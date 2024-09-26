namespace SportShopC_.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository _userRepository { get; }
        IOrderRepository _orderRepository { get; }
        IOrderProductRepository _orderProductRepository { get; }
        void Commit();
        void Dispose();
    }
}
