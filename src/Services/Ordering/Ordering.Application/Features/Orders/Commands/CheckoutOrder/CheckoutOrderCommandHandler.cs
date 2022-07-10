using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailServ;
        private readonly ILogger<CheckoutOrderCommandHandler> _logging;


        public CheckoutOrderCommandHandler(IOrderRepository repo,
                                           IMapper mapper,
                                           IEmailService emailServ,
                                           ILogger<CheckoutOrderCommandHandler> logging)
        {
            _repo = repo;   
            _mapper = mapper;   
            _emailServ = emailServ; 
            _logging = logging;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEnt = _mapper.Map<Order>(request);

            //var newOrder = await _repo.AddAsync(orderEnt);

            //await SendMail(newOrder);

            await _repo.AddAsync(orderEnt);
            await SendMail(orderEnt);

            _logging.LogInformation($"Order {orderEnt.Id} is successfully created.");

            return orderEnt.Id;
        }
        
        public async Task SendMail(Order ord)
        {
            var email = new Email
            {
                To = "gokhmaz.mahraamov@gmai.com",
                Body = "Order was created",
                Subject = "Nre email sertification"
            };

            try
            {
                await _emailServ.SendEmail(email);
            }
            catch
            {
                _logging.LogInformation($"Order {ord.Id} failed due to an error with the service");
            }
        }
    }
}
