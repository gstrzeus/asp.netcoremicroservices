using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCard> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrWhiteSpace(basket)) return null;

            return JsonConvert.DeserializeObject<ShoppingCard>(basket); 
        }

        public async Task<ShoppingCard> UpdateBasket(ShoppingCard shoppingCard)
        {
            await _redisCache.SetStringAsync(shoppingCard.UserName, JsonConvert.SerializeObject(shoppingCard));

            return await GetBasket(shoppingCard.UserName);
        }
    }
}
