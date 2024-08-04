using NUnit.Framework;
using System;
using InfotrackAPI.Models;

namespace InfortrackAPI.UnitTests.Models
{
    [TestFixture]
    public class BookingDetailTests
    {
        [Test]
        public void BookingDetail_SetProperties_ShouldGetSameProperties()
        {
            // Arrange
            var bookingTime = DateTime.Now;
            var name = "John Doe";
            var bookingId = Guid.NewGuid();
            var bookingDetail = new BookingDetail();

            // Act
            bookingDetail.BookingTime = bookingTime;
            bookingDetail.Name = name;
            bookingDetail.BookingId = bookingId;

            // Assert
            Assert.AreEqual(bookingTime, bookingDetail.BookingTime);
            Assert.AreEqual(name, bookingDetail.Name);
            Assert.AreEqual(bookingId, bookingDetail.BookingId);
        }

        [Test]
        public void BookingDetail_ConstructorInitialization_ShouldSetProperties()
        {
            // Arrange
            var bookingTime = DateTime.Now;
            var name = "Jane Doe";
            var bookingId = Guid.NewGuid();
            var bookingDetail = new BookingDetail
            {
                BookingTime = bookingTime,
                Name = name,
                BookingId = bookingId
            };

            // Assert
            Assert.AreEqual(bookingTime, bookingDetail.BookingTime);
            Assert.AreEqual(name, bookingDetail.Name);
            Assert.AreEqual(bookingId, bookingDetail.BookingId);
        }

        [Test]
        public void BookingDetail_DefaultConstructor_ShouldInitializeProperties()
        {
            // Act
            var bookingDetail = new BookingDetail();

            // Assert
            Assert.AreEqual(default(DateTime), bookingDetail.BookingTime);
            Assert.IsNull(bookingDetail.Name);
            Assert.AreEqual(default(Guid), bookingDetail.BookingId);
        }
    }
}
