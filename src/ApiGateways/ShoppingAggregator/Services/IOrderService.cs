namespace ShoppingAggregator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Models.OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
