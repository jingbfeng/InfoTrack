using System;
using InfotrackAPI.Models;

namespace InfotrackAPI.Services
{
	public interface IBookingService
	{
        bool CheckIfAllBookingReserved(DateTime bookingTime);

        void AddBooking(BookingDetail bookingDetail);

        BookingDetail FindBooking(Guid bookingId);
    }
}

