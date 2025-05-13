namespace EventEase3.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string email { get; set; } = "";

        public string eventName { get; set; } = "";

        public string venue { get; set; } = "";
        public string eventDate { get; set; } = "";

        public DateTime createdAt { get; set; }

    }
}
