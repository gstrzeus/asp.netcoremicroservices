using Discount.Grpcs.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcServices
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _grpcClient;
        public DiscountGrpcServices(DiscountProtoService.DiscountProtoServiceClient communi)
        {
            _grpcClient = communi;
        }

        public async Task<CouponModel> GetDiscount(string productnName)
        {
            var discountRequest = new GetDiscountRequest
            {
                ProductName = productnName
            };

            return await _grpcClient.GetDiscountAsync(discountRequest);
        }
    }
}
