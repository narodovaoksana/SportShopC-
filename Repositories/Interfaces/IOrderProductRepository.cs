using SportShopC_.Entities;

namespace SportShopC_.Repositories.Interfaces
{
    public interface IOrderProductRepository : IGenericRepository<OrderProduct>
    {
        Task<IEnumerable<OrderProduct>> GetProductsByOrderIdAsync(int orderId);
    }
}
