using ABCHotel.Data;
using ABCHotel.Repositories;

namespace ABCHotel.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRoomsRepository _roomRepo;
        private readonly ICouponRepository _couponRepo;
        public BookingService(IRoomsRepository roomRepo, ICouponRepository couponRepo)
        {
            _roomRepo = roomRepo;
            _couponRepo = couponRepo;
        }

        public bool IsBookingValid(int roomId, Booking booking)
        {
            var guestIsSmoking = booking.IsSmoking;
            var guestIsBringingPets = booking.HasPets;
            var numberOfGuests = booking.NumberOfGuests;
            if (guestIsSmoking)
            {
                return false;
            }
            var room = _roomRepo.GetRoom(roomId);
            if (guestIsBringingPets && !room.ArePetsAllowed)
            {
                return false;
            }

            if(numberOfGuests > room.Capacity)
            {
                return false;
            }
            return true;
        }

        public decimal CalculateBookingPrice(Booking booking)
        {
          
            var room = _roomRepo.GetRoom(booking.Id);

            var numberOfNights = (booking.CheckOutDate - booking.CheckInDate).Days;
            var price = room.Rate * numberOfNights;

            if (string.IsNullOrEmpty(booking.CouponCode)) return price;
           
            var discount = _couponRepo.GetCoupon(booking.CouponCode).PercentageDiscount;
                price = price - price * discount / 100;
           

            return price;
        }
    }
}
