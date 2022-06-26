using Basket.API.Entities;
using Basket.API.GrpcServices;
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

        private readonly DiscountGrpcServices _discountRepo;

        public BasketController(IBasketRepository basket,
                                DiscountGrpcServices discount)
        {
            _baskterRepo = basket;
            _discountRepo = discount;   
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
            foreach (var item in basket.Items)
            {
                var coupon = await _discountRepo.GetDiscount(item.ProductNamr);
                item.Price -= coupon.Amount;
            }

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
