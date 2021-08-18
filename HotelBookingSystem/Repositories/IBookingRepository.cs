using HotelBookingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSystem.Repositories
{
    public interface IBookingRepository
    {
        public Booking GetBookingForRoom(int room, DateTime date);
        public IEnumerable<Booking> GetBookingsForDate(DateTime date);
        public void AddBooking(string guest, int room, DateTime date);
    }
}
