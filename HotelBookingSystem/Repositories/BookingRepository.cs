using HotelBookingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HotelBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private IThreadSafeRepositoryWrapper<Booking> _bookingWrapper;

        public BookingRepository(IThreadSafeRepositoryWrapper<Booking> bookingWrapper)
        {
            _bookingWrapper = bookingWrapper;
        }

        public Booking GetBookingForRoom(int room, DateTime date)
        {
            return _bookingWrapper.ReadWrapper(() =>
            {
                return TestHotelDb.Bookings
                .FirstOrDefault(b => b.Room == room && b.Date == date);
            });
        }

        public IEnumerable<Booking> GetBookingsForDate(DateTime date)
        {
            return _bookingWrapper.ReadManyWrapper(() =>
            {
                return TestHotelDb.Bookings
                .Where(b => b.Date == date);
            });
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            _bookingWrapper.WriteWrapper(() =>
            {
                var currentBooking = TestHotelDb.Bookings.FirstOrDefault(b => b.Room == room && b.Date == date);

                if (currentBooking != null){
                    throw new ArgumentException("The chosen room is not available for booking");
                }

                TestHotelDb.Bookings.Add(new Booking
                {
                    Date = date,
                    Guest = guest,
                    Room = room
                });
            });
        }
    }
}
