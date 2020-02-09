using ABCHotel.Data;

namespace ABCHotel.Services
{
    public interface IBookingService
    {
       decimal CalculateBookingPrice(Booking booking);
    }
}