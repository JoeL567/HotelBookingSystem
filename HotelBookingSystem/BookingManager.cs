using HotelBookingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelBookingSystem
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        public BookingManager(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public void AddBooking(string guest, int room, DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Cannot book a room for date in the past");
            }

            var roomToUse = _roomRepository.GetRoom(room);

            if (roomToUse == null)
            {
                throw new ArgumentException("The chosen room does not exist");
            }

            _bookingRepository.AddBooking(guest, room, date);


        }

        public IEnumerable<int> GetAvailableRooms(DateTime date)
        {
            var bookings = _bookingRepository.GetBookingsForDate(date).Select(b => b.Room);

            return _roomRepository
                .GetRooms()
                .Where(r => !bookings.Contains(r.RoomNumber))
                .Select(r => r.RoomNumber).ToList();
        }

        public bool IsRoomAvailable(int room, DateTime date)
        {
            var roomToUse = _roomRepository.GetRoom(room);

            if(roomToUse == null)
            {
                throw new ArgumentException("The chosen room does not exist");
            }

            var bookings = _bookingRepository.GetBookingForRoom(room, date);

            return bookings == null;
        }
    }
}
