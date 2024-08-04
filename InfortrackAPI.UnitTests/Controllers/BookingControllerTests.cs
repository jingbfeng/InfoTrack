using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using InfotrackAPI.Controllers;
using InfotrackAPI.Models;
using InfotrackAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfotrackAPI.Tests
{
    [TestFixture]
    public class BookingControllerTests
    {
        private Mock<ILogger<BookingController>> _loggerMock;
        private Mock<IBookingService> _bookingServiceMock;
        private BookingController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<BookingController>>();
            _bookingServiceMock = new Mock<IBookingService>();
            _controller = new BookingController(_loggerMock.Object, _bookingServiceMock.Object);
        }

        [Test]
        public void GetBookingDetail_ReturnsNotFound_WhenBookingIsNull()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            _bookingServiceMock.Setup(service => service.FindBooking(bookingId)).Returns((BookingDetail)null);

            // Act
            var result = _controller.GetBookingDetail(bookingId) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Booking not found.", result.Value);
        }

        [Test]
        public void GetBookingDetail_ReturnsOk_WhenBookingIsFound()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var booking = new BookingDetail
            {
                BookingId = bookingId,
                BookingTime = DateTime.Now,
                Name = "John Doe"
            };
            _bookingServiceMock.Setup(service => service.FindBooking(bookingId)).Returns(booking);

            // Act
            var result = _controller.GetBookingDetail(bookingId) as OkObjectResult;
            var bookingDetail = result.Value as BookingDetail;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(bookingId, bookingDetail.BookingId);
            Assert.AreEqual(booking.BookingTime, bookingDetail.BookingTime);
            Assert.AreEqual(booking.Name, bookingDetail.Name);
        }

        [Test]
        public void UpdateBooking_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var bookingUpdate = new BookingUpdate
            {
                BookingTime = DateTime.Now,
                Name = "John Doe"
            };

            // Act
            var result = _controller.UpdateBooking(bookingUpdate) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void UpdateBooking_ReturnsBadRequest_WhenBookingTimeIsOutOfHours()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                BookingTime = DateTime.Today.AddHours(18),
                Name = "John Doe"
            };

            // Act
            var result = _controller.UpdateBooking(bookingUpdate) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Booking for out of hours times is not valid.", result.Value);
        }

        [Test]
        public void UpdateBooking_ReturnsConflict_WhenAllBookingsAreReserved()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                BookingTime = DateTime.Now,
                Name = "John Doe"
            };
            _bookingServiceMock.Setup(service => service.CheckIfAllBookingReserved(It.IsAny<DateTime>())).Returns(true);

            // Act
            var result = _controller.UpdateBooking(bookingUpdate) as ConflictObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(409, result.StatusCode);
            Assert.AreEqual("All bookings are reserved within one hour.", result.Value);
        }

        [Test]
        public void UpdateBooking_ReturnsOk_WhenBookingIsValid()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                BookingTime = DateTime.Now,
                Name = "John Doe"
            };
            _bookingServiceMock.Setup(service => service.CheckIfAllBookingReserved(It.IsAny<DateTime>())).Returns(false);

            // Act
            var result = _controller.UpdateBooking(bookingUpdate) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}


