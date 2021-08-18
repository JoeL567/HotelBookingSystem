using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSystem.Entities
{
    public class Booking : IEntity
    {
        public string Guest { get; set; }
        public int Room { get; set; }
        public DateTime Date { get; set; }
    }
}
