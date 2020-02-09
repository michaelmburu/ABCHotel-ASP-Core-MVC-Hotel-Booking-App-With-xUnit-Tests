using System.Collections.Generic;
using ABCHotel.Data;

namespace ABCHotel.Repositories
{
    public interface IBookingsRepository
    {
        IList<Booking> GetBookings(int roomId);
    }
}