using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetQueryList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        public GetOrderListQueryHandler(IOrderRepository orderRepo,
                                        IMapper mapper)
        {
            _repo = orderRepo;
            _mapper = mapper;
        }
        public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var modelOrders = await _repo.GetOrderByUserName(request.UserName);

            return _mapper.Map<List<OrdersVm>>(modelOrders);
        }
    }
}
