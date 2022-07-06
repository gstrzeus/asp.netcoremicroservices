using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
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

        private readonly IMapper _mapper;

        private readonly IPublishEndpoint _publishPoint;

        public BasketController(IBasketRepository basket,
                                DiscountGrpcServices discount,
                                IMapper map,
                                IPublishEndpoint publish)
        {
            _baskterRepo = basket;
            _discountRepo = discount;   
            _mapper = map;
            _publishPoint = publish;
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

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basket)
        {
            var currentBasket = await _baskterRepo.GetBasket(basket.UserName);

            if (currentBasket is null) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basket);
            eventMessage.TotalPrice = currentBasket.TotalPrice;

            await Task.WhenAll(_publishPoint.Publish(eventMessage), _baskterRepo.DeleteBasket(basket.UserName));

            return Accepted();
        }

    }
}
