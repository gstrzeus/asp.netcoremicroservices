using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class BasketService : IBasketService
    {
        private HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client;
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var respone = await _client.PostAsJson("/Basket/Checkout", model);

            if (!respone.IsSuccessStatusCode) throw new System.Exception("Something went wrong when calling api.");
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/Basket/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await _client.PostAsJson("/Basket", model);
            
            if (response.IsSuccessStatusCode) return await response.ReadContentAs<BasketModel>();
            
            throw new System.Exception("Sometingn went wrong, when call api.");
        }
    }
}
