namespace SportShopC_.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
