using HotelBookingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSystem.Repositories
{
    public interface IRoomRepository
    {
        public Room GetRoom(int room);
        public IEnumerable<Room> GetRooms();
    }
}
