using System.Collections.Generic;
using ABCHotel.Data;
using ABCHotel.Repositories;

namespace ABCHotel.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomsRepository _roomRepo;

        public RoomService(IRoomsRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public IList<Room> GetAllRooms()
        {
            return _roomRepo.GetRooms(); 
        }
    }
}
