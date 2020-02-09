using ABCHotel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCHotel.Repositories
{
    public interface ICouponRepository
    {
        string GetCoupon(Coupon code);
        Coupon GetCoupon(string coupon);
    }
}
