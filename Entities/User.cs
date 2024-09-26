namespace SportShopC_.Entities
{
    public class User : BaseEntity
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }
        public String PhoneNumber { get; set; }

        public String Address { get; set; }

        public String Role { get; set; }
    }
}
