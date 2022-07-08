using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}
