using AutoMapper;
using Discount.Grpcs.Entities;
using Discount.Grpcs.Protos;

namespace Discount.Grpcs.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
