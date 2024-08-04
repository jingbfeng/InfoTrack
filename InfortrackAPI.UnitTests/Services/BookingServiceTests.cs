using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using InfotrackAPI.Services;
using InfotrackAPI.Models;

namespace InfortrackAPI.UnitTests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private BookingService _bookingService;
        private Mock<ILogger<BookingService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(_loggerMock.Object);
        }

        [Test]
        public void CheckIfAllBookingReserved_ShouldReturnTrue_WhenMaxBookingsInOneHour()
        {
            // Arrange
            DateTime bookingTime = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                _bookingService.AddBooking(new BookingDetail { BookingId = Guid.NewGuid(), BookingTime = bookingTime.AddMinutes(i * 15) });
            }

            // Act
            var result = _bookingService.CheckIfAllBookingReserved(bookingTime);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckIfAllBookingReserved_ShouldReturnFalse_WhenLessThanMaxBookingsInOneHour()
        {
            // Arrange
            DateTime bookingTime = DateTime.Now;
            for (int i = 0; i < 3; i++)
            {
                _bookingService.AddBooking(new BookingDetail { BookingId = Guid.NewGuid(), BookingTime = bookingTime.AddMinutes(i * 20) });
            }

            // Act
            var result = _bookingService.CheckIfAllBookingReserved(bookingTime);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddBooking_ShouldAddBookingToList()
        {
            // Arrange
            var bookingDetail = new BookingDetail { BookingId = Guid.NewGuid(), BookingTime = DateTime.Now };

            // Act
            _bookingService.AddBooking(bookingDetail);
            var result = _bookingService.FindBooking(bookingDetail.BookingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingDetail.BookingId, result.BookingId);
        }

        [Test]
        public void FindBooking_ShouldReturnNull_WhenBookingDoesNotExist()
        {
            // Arrange
            var bookingId = Guid.NewGuid();

            // Act
            var result = _bookingService.FindBooking(bookingId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void FindBooking_ShouldReturnBookingDetail_WhenBookingExists()
        {
            // Arrange
            var bookingDetail = new BookingDetail { BookingId = Guid.NewGuid(), BookingTime = DateTime.Now };
            _bookingService.AddBooking(bookingDetail);

            // Act
            var result = _bookingService.FindBooking(bookingDetail.BookingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingDetail.BookingId, result.BookingId);
        }
    }
}


