using HotelBookingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSystem.Repositories
{
    public static class TestHotelDb
    {
        public static List<Room> Rooms { get; set; }
        public static List<Booking> Bookings { get; set; }

        public static void SeedDatabase()
        {
            Rooms = new List<Room>();
            Bookings = new List<Booking>();

            Bookings.AddRange(new List<Booking>
            {
                new Booking
                {
                    Guest = "Bobby",
                    Room = 101,
                    Date = new DateTime(2021, 08, 17)
                },
                new Booking
                {
                    Guest = "Toast",
                    Room = 101,
                    Date = new DateTime(2021, 08, 18)
                },
                new Booking
                {
                    Guest = "Longman",
                    Room = 102,
                    Date = new DateTime(2021, 08, 15)
                },
                new Booking
                {
                    Guest = "Harrison",
                    Room = 102,
                    Date = new DateTime(2021, 08, 16)
                },
                    new Booking
                {
                    Guest = "Smith",
                    Room = 201,
                    Date = new DateTime(2021, 08, 17)
                },
                new Booking
                {
                    Guest = "White",
                    Room = 201,
                    Date = new DateTime(2021, 08, 18)
                }
            });

            Rooms.AddRange(new List<Room>
            {
                new Room
                {
                    RoomNumber = 101
                },
                new Room
                {
                    RoomNumber = 102
                },
                new Room
                {
                    RoomNumber = 201
                },
                new Room
                {
                    RoomNumber = 203
                }
            });
        }
    }
}


