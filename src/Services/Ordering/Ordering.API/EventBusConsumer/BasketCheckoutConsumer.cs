using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _repo;
        private readonly ILogger<BasketCheckoutConsumer> _log;

        public BasketCheckoutConsumer(IMapper mapper, IMediator repo, ILogger<BasketCheckoutConsumer> log)
        {
            _mapper = mapper;
            _repo = repo;
            _log = log;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);

            var id = await _repo.Send(command);

            _log.LogInformation("BasketCheckout consumed successfully. Created order Id by : " + id);
        }
    }
}
