using HotelBookingSystem;
using HotelBookingSystem.Entities;
using HotelBookingSystem.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSystem.Tests
{
    [TestFixture]
    public class BookingManagerTests
    {
        private BookingRepository bookingRepo;
        private RoomRepository roomRepo;
        private BookingManager bm;
        private DateTime testDate;

        [SetUp]
        public void Setup()
        {
            bookingRepo = new BookingRepository(new ThreadSafeRepositoryWrapper<Booking>());
            roomRepo = new RoomRepository(new ThreadSafeRepositoryWrapper<Room>());
            bm = new BookingManager(bookingRepo, roomRepo);
            testDate = DateTime.Now.AddDays(1);
            TestHotelDb.SeedDatabase();
        }
        [TearDown]
        public void TearDown()
        {
            TestHotelDb.Bookings = new List<Booking>();
            TestHotelDb.Rooms = new List<Room>();
        }

        [Test]
        public void IsRoomAvailable_Returns_True_If_Room_Is_Available()
        {
            var isAvailable = bm.IsRoomAvailable(101, testDate);
            Assert.That(isAvailable, Is.True);
        }

        [Test]
        public void IsRoomAvailable_Returns_False_If_Room_Is_Not_Available()
        {
            testDate = new DateTime(2021, 08, 17);
            var isAvailable = bm.IsRoomAvailable(101, testDate);
            Assert.That(isAvailable, Is.False);
        }

        [Test]
        public void IsRoomAvailable_Throws_ArgumentError_If_Room_Does_Not_Exist()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => { bm.IsRoomAvailable(1234567, testDate); });
            Assert.That(ex.Message, Is.EqualTo("The chosen room does not exist"));
        }

        [Test]
        public void AddBooking_Throws_ArgumentError_If_Date_Is_In_Past()
        {
            testDate = new DateTime(2010, 01, 01);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => { bm.AddBooking("guestname", 101, testDate); });
            Assert.That(ex.Message, Is.EqualTo("Cannot book a room for date in the past"));
        }

        [Test]
        public void AddBooking_Throws_ArgumentError_If_Room_Does_Not_Exist()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => { bm.AddBooking("guestname", 1234567, testDate); });
            Assert.That(ex.Message, Is.EqualTo("The chosen room does not exist"));
        }

        [Test]
        public void AddBooking_Throws_ArgumentError_If_Room_Is_Booked_On_Date()
        {
            // first create a booking then try to add a second
            bm.AddBooking("guest1", 101, testDate);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => { bm.AddBooking("guestname", 101, testDate); });
            Assert.That(ex.Message, Is.EqualTo("The chosen room is not available for booking"));
        }

        [Test]
        public void AddBooking_Does_Not_Double_Book_When_Accessed_By_Multiple_Threads()
        {
            List<Task> tasks = new List<Task>();
            List<ArgumentException> exceptions = new List<ArgumentException>();

            for(int i=0; i<10; i++)
            {
                Task t = new Task(() => bm.AddBooking("guestname", 101, testDate));
                tasks.Add(t);
                t.Start(); 
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    if (e is ArgumentException)
                    {
                        var argEx = (ArgumentException)e;
                        exceptions.Add(argEx);
                    }
                    // Rethrow any other exception.
                    else
                    {
                        throw e;
                    }
                }
            }

            var newbooking = TestHotelDb.Bookings.Where(bm => bm.Guest == "guestname" && bm.Date == testDate && bm.Room == 101);
            Assert.That(newbooking, Has.Exactly(1).Items);
            Assert.That(exceptions, Has.Exactly(9).Items);
            Assert.That(exceptions, Is.All.Matches<ArgumentException>(ex => ex.Message == "The chosen room is not available for booking"));
        }

        [Test]
        public void AddBooking_Adds_Booking_If_Date_And_Room_Are_Valid()
        {
            bm.AddBooking("guest1", 101, testDate);
            var newbooking = TestHotelDb.Bookings.FirstOrDefault(bm => bm.Guest == "guest1" && bm.Date == testDate && bm.Room == 101);
            Assert.That(newbooking, Is.Not.Null);
        }

        [Test]
        public void GetAvailableRooms_Returns_List_Of_Available_Rooms_For_Date()
        {
            var roomList = bm.GetAvailableRooms(testDate);
            Assert.That(roomList, Is.EqualTo(TestHotelDb.Rooms.Select(r => r.RoomNumber)));
        }
    }
}