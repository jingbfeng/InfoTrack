using System;
namespace InfotrackAPI.Models
{
	public class BookingDetail
	{
        public DateTime BookingTime { get; set; }

        public string Name { get; set; }

        public Guid BookingId { get; set; }
    }
}

