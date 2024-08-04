using System;
using InfotrackAPI.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace InfotrackAPI.Services
{
	public class BookingService: IBookingService
	{
        private List<BookingDetail> Bookings = new List<BookingDetail>();
        private const int MaxBookingsPerHour = 4;
        private readonly ILogger<BookingService> _logger;


        public BookingService(ILogger<BookingService> logger)
        {
            _logger = logger;
        }

        public bool CheckIfAllBookingReserved(DateTime bookingTime)
        {
            DateTime startTime = bookingTime.AddHours(-1);
            DateTime endTime = bookingTime.AddHours(1);
            int bookingCount = 0;

            foreach (var booking in Bookings)
            {
                if (booking.BookingTime >= startTime && booking.BookingTime <= endTime)
                {
                    bookingCount++;
                }

                if (bookingCount >= MaxBookingsPerHour)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddBooking(BookingDetail bookingDetail)
        {
            Bookings.Add(bookingDetail);
        }

        public BookingDetail FindBooking(Guid bookingId)
        {
            return Bookings.Find(b => b.BookingId == bookingId);
        }
    }
}

