using System.Collections.Generic;
using ABCHotel.Data;

namespace ABCHotel.Repositories
{
    public interface IRoomsRepository
    {
        IList<Room> GetRooms();
        Room GetRoom(int id);
    }
}