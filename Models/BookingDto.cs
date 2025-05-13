using System.ComponentModel.DataAnnotations;

namespace EventEase3.Models
{
    public class BookingDto
    {
        //this what makes the error handling for making sure the form is filled before submitting to the database
        [Required]
        public string email { get; set; } = "";

        [Required]
        public string eventName { get; set; } = "";

        [Required]
        public string venue { get; set; } = "";

        [Required]
        public string eventDate { get; set; } = "";


    }
}
