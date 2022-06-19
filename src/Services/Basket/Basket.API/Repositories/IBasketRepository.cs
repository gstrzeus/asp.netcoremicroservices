using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCard> GetBasket(string userName);

        Task<ShoppingCard> UpdateBasket(ShoppingCard shoppingCard);

        Task DeleteBasket(string userName);
    }
}
