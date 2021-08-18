using HotelBookingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelBookingSystem.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private IThreadSafeRepositoryWrapper<Room> _wrapper;

        public RoomRepository(IThreadSafeRepositoryWrapper<Room> wrapper)
        {
            _wrapper = wrapper;
        }

        public Room GetRoom(int room)
        {
            return _wrapper.ReadWrapper(() =>
            {
                return TestHotelDb.Rooms
                .FirstOrDefault(r => r.RoomNumber == room);
            });
        }

        public IEnumerable<Room> GetRooms()
        {
            return _wrapper.ReadManyWrapper(() =>
            {
                return TestHotelDb.Rooms;
            });
        }
    }
}
