using HotelBookingSystem.Entities;
using HotelBookingSystem.Repositories;
using System;

namespace HotelBookingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            TestHotelDb.SeedDatabase();

            var bookingrepo = new BookingRepository(new ThreadSafeRepositoryWrapper<Booking>());
            var roomrepo = new RoomRepository(new ThreadSafeRepositoryWrapper<Room>());
            var bm = new BookingManager(bookingrepo, roomrepo);
            var today = new DateTime(2021, 9, 17);
            Console.WriteLine(bm.GetAvailableRooms(today));
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs true 
            bm.AddBooking("Patel", 101, today);
            Console.WriteLine(bm.IsRoomAvailable(101, today)); // outputs false 
            bm.AddBooking("Li", 101, today); // throws an exception

        }
    }
}
