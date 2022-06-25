using AutoMapper;
using Discount.Grpcs.Entities;
using Discount.Grpcs.Protos;
using Discount.Grpcs.Repositories;
using Grpc.Core;

namespace Discount.Grpcs.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repo,
                               IMapper map,
                               ILogger<DiscountService> logger)
        {
            _repo = repo;
            _mapper = map;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repo.GetDiscount(request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, String.Format("Discount with {0}- product is not found", request.ProductName)));
            }
            _logger.LogInformation(String.Format("Discount is retrieved for ProductName: {0}, amoun: {1}", coupon.ProductName, coupon.Amount));

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repo.CreateDiscount(coupon);

            _logger.LogInformation(String.Format("Discount is successfully created. Product name : {0}", coupon.ProductName));

            request.Coupon.Id = coupon.Id;

            return request.Coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repo.UpdateDiscount(coupon);

            _logger.LogInformation(String.Format("Discount is successfully updated. Product name : {0}", coupon.ProductName));

            return request.Coupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repo.DeleteDiscount(request.ProductName);

            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }
    }
}
