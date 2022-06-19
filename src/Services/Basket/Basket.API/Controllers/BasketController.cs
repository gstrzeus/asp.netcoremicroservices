using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketRepository _baskterRepo;

        public BasketController(IBasketRepository basket)
        {
            _baskterRepo = basket;
        }


        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCard>> GetBasket(string userName)
        {
            var basket = await _baskterRepo.GetBasket(userName);

            return Ok(basket ?? new (userName));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCard), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCard>> UpdateBasket([FromBody] ShoppingCard basket)
        {
            return Ok(await _baskterRepo.UpdateBasket(basket));
        }


        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _baskterRepo.DeleteBasket(userName);
            return Ok();
        }

    }
}
