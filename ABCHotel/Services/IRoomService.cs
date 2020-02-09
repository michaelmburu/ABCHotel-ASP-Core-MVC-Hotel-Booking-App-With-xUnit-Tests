using System.Collections.Generic;
using ABCHotel.Data;

namespace ABCHotel.Services
{
    public interface IRoomService
    {
        IList<Room> GetAllRooms();
    }
}