using InfotrackAPI.Models;
using InfotrackAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfotrackAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _bookingService;

    public BookingController(ILogger<BookingController> logger, IBookingService booingService)
    {
        _logger = logger;
        _bookingService = booingService;
    }

    [HttpGet("{bookingId}", Name = "GetBookingDetail")]
    public IActionResult GetBookingDetail(Guid bookingId)
    {
        var booking = _bookingService.FindBooking(bookingId);
        if (booking == null)
        {
            return NotFound("Booking not found.");
        }

        var bookingDetail = new BookingDetail
        {
            BookingId = booking.BookingId,
            BookingTime = booking.BookingTime,
            Name = booking.Name
        };

        return Ok(bookingDetail);
    }

    [HttpPost(Name = "UpdateBooking")]
    public IActionResult UpdateBooking([FromBody] BookingUpdate bookingUpdate)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Assuming business hours are from 09:00 to 17:00
        TimeSpan startTime = new TimeSpan(9, 0, 0);
        TimeSpan endTime = new TimeSpan(17, 0, 0);

        if (bookingUpdate.BookingTime.TimeOfDay < startTime || bookingUpdate.BookingTime.TimeOfDay > endTime)
        {
            return BadRequest("Booking for out of hours times is not valid.");
        }

        // Check for reserved settlements at booking time (this is a placeholder)
        bool allBookingReserved = _bookingService.CheckIfAllBookingReserved(bookingUpdate.BookingTime);

        if (allBookingReserved)
        {
            return Conflict("All bookings are reserved within one hour.");
        }

        //map bookingUpdate to bookingDetail
        BookingDetail bookingDetail = new BookingDetail { BookingTime = bookingUpdate.BookingTime, BookingId = Guid.NewGuid(), Name = bookingUpdate.Name };

        // Add booking to the momery list
        _bookingService.AddBooking(bookingDetail);

        // If everything is valid, return a success status with booking ID and message in response body
        return Ok(new { bookingId = bookingDetail.BookingId });
    }

}

