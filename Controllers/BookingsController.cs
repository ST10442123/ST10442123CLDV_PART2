using EventEase3.Models;
using EventEase3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


//controllers for create, Read, edit and delete are in here
namespace EventEase3.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext context;

        public BookingsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //public IActionResult Index()
        //{
        //    var bookings = context.Bookings.OrderByDescending(p => p.Id).ToList();
        //    return View(bookings);
        //}
        public IActionResult Index()
        {
            var allBookings = context.Bookings.ToList();
            ViewBag.AllBookings = allBookings;
            ViewData["SearchPerformed"] = false;
            return View(allBookings);
        }

        //public IActionResult Search(string searchTerm)
        //{
        //    var allBookings = context.Bookings.ToList();
        //    ViewBag.AllBookings = allBookings;

        //    var searchResults = context.Bookings
        //        .Where(b =>
        //            b.Id.ToString().Contains(searchTerm) ||
        //            b.eventName.ToLower().Contains(searchTerm.ToLower()))
        //        .ToList();

        //    ViewData["SearchPerformed"] = true;
        //    return View("Index", searchResults);
        //}

        public IActionResult Search(string searchTerm)
        {
            var allBookings = context.Bookings.ToList();
            ViewBag.AllBookings = allBookings;
            ViewData["SearchPerformed"] = true;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ModelState.AddModelError(string.Empty, "Please enter a search term.");
                return View("Index", new List<Booking>());
            }

            var searchResults = context.Bookings
                .Where(b =>
                    b.Id.ToString().Contains(searchTerm) ||
                    b.eventName.ToLower().Contains(searchTerm.ToLower()))
                .ToList();

            return View("Index", searchResults);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookingDto bookingDto)
        {
            if (ModelState.IsValid)
            {
                // Check for double booking: same venue and event date
                var existingBooking = context.Bookings
                    .FirstOrDefault(b => b.venue == bookingDto.venue && b.eventDate == bookingDto.eventDate);

                if (existingBooking != null)
                {
                    // Add an error to the model state if double booking is detected
                    ModelState.AddModelError("eventDate", "A booking for this venue already exists for this day.");
                    return View(bookingDto); // Return to the form with validation error
                }

                // Proceed with creating the booking if no double booking is found
                var booking = new Booking
                {
                    email = bookingDto.email,
                    eventName = bookingDto.eventName,
                    venue = bookingDto.venue,
                    eventDate = bookingDto.eventDate,
                    createdAt = DateTime.Now
                };

                context.Bookings.Add(booking);
                context.SaveChanges();

                // Redirect to index or another page after successful submission
                return RedirectToAction("Index");
            }

            // Return the form with validation errors if ModelState is invalid
            return View(bookingDto);
        }


        //public IActionResult Create(BookingDto bookingDto)
        //{
        //    if (bookingDto.eventDate == null)
        //    {
        //        ModelState.AddModelError("eventDate", "Event date is required");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(bookingDto);
        //    }

        //    //to save to the database
        //    Booking booking = new Booking
        //    {
        //        email = bookingDto.email,
        //        eventName = bookingDto.eventName,
        //        venue = bookingDto.venue,
        //        eventDate = bookingDto.eventDate,
        //        createdAt = DateTime.Now
        //    };

        //    context.Bookings.Add(booking);
        //    context.SaveChanges();


        //    return RedirectToAction("Index", "Bookings");
        //}

        public IActionResult Edit(int id)
        {
            var booking = context.Bookings.Find(id);

            if (booking == null)
            {
                return RedirectToAction("Index", "Bookings");
            }

            //create the bookingDto from booking
            var bookingDto = new BookingDto
            {

                email = booking.email,
                eventName = booking.eventName,
                venue = booking.venue,
                eventDate = booking.eventDate
            };

            ViewData["BookingId"] = booking.Id;
            ViewData["CreatedAt"] = booking.createdAt.ToString("MM/dd/yyyy");

            return View(bookingDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, BookingDto bookingDto)
        {

            var booking = context.Bookings.Find(id);

            if (booking == null)
            {
                return RedirectToAction("Index", "Bookings");
            }

            if (!ModelState.IsValid)
            {
                ViewData["BookingId"] = booking.Id;
                ViewData["CreatedAt"] = booking.createdAt.ToString("MM/dd/yyyy");
                return View(bookingDto);
            }

            //to update the datdabase with teh new booking ifo

            booking.email = bookingDto.email;
            booking.eventName = bookingDto.eventName;
            booking.venue = bookingDto.venue;
            booking.eventDate = bookingDto.eventDate;


            context.SaveChanges();

            return RedirectToAction("Index", "Bookings");


        }

        public IActionResult Delete(int id)
        {
            var booking = context.Bookings.Find(id);
            if (booking == null)
            {
                return RedirectToAction("Index", "Bookings");
            }

            context.Bookings.Remove(booking);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Bookings");
        }

        public ActionResult Venues()
        {
            return View();
        }


        public ActionResult Events()
        {
            return View();
        }


    }
}
