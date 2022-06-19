namespace Basket.API.Entities
{
    public class ShoppingCard
    {
        public string UserName { get; set; }
        public List<ShoppingCardItem> Items { get; set; } = new();

        public ShoppingCard(string userName) => UserName = userName;

        public decimal TotalPrice
        {
            get  => Items.Sum(item => item.Quantity * item.Price);
        }
    }
}
