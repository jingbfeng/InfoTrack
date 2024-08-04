using System;
using System.ComponentModel.DataAnnotations;
namespace InfotrackAPI.Models
{
	public class BookingUpdate
	{
        [Required]
        [DataType(DataType.Time)]
        [CustomValidation(typeof(BookingUpdate), nameof(ValidateBookingTime))]
        public DateTime BookingTime { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "The Name property should be a non-empty string.")]
        public string Name { get; set; }

        public static ValidationResult ValidateBookingTime(DateTime bookingTime, ValidationContext context)
        {
            if (bookingTime.TimeOfDay < new TimeSpan(0, 0, 0) || bookingTime.TimeOfDay > new TimeSpan(23, 59, 0))
            {
                return new ValidationResult("The bookingTime property should be a 24-hour time (00:00 - 23:59).");
            }
            return ValidationResult.Success;
        }

    }
}

