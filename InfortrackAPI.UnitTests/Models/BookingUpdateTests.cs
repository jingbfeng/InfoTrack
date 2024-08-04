using InfotrackAPI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfotrackAPI.Tests
{
    [TestFixture]
    public class BookingUpdateTests
    {
        [Test]
        public void ValidateBookingTime_ValidTime_ReturnsSuccess()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                BookingTime = DateTime.Today.AddHours(10), // Example valid time
                Name = "John Doe" // Example valid name
            };

            // Act
            var validationResults = ValidateModel(bookingUpdate);

            // Assert
            Assert.IsEmpty(validationResults);
        }

        //[Test]
        //public void ValidateBookingTime_InvalidTime_ReturnsValidationError()
        //{
        //    // Arrange
        //    var bookingUpdate = new BookingUpdate
        //    {
        //        BookingTime = DateTime.Today.AddHours(25), // Example invalid time
        //        Name = "John Doe" // Example valid name
        //    };

        //    // Act
        //    var validationResults = ValidateModel(bookingUpdate);

        //    // Assert
        //    Assert.IsNotEmpty(validationResults);
        //    Assert.AreEqual("The bookingTime property should be a 24-hour time (00:00 - 23:59).", validationResults[0].ErrorMessage);
        //}

        [Test]
        public void ValidateName_ValidName_ReturnsSuccess()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                Name = "John Doe", // Example valid name
                BookingTime = DateTime.Today.AddHours(10), // Example valid time
            };

            // Act
            var validationResults = ValidateModel(bookingUpdate);

            // Assert
            Assert.IsEmpty(validationResults);
        }

        [Test]
        public void ValidateName_EmptyName_ReturnsValidationError()
        {
            // Arrange
            var bookingUpdate = new BookingUpdate
            {
                Name = "", // Example invalid name
                BookingTime = DateTime.Today.AddHours(10), // Example valid time
            };

            // Act
            var validationResults = ValidateModel(bookingUpdate);

            // Assert
            Assert.IsNotEmpty(validationResults);
            Assert.AreEqual("The Name field is required.", validationResults[0].ErrorMessage);
        }

        private List<ValidationResult> ValidateModel(BookingUpdate bookingUpdate)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(bookingUpdate);
            Validator.TryValidateObject(bookingUpdate, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
