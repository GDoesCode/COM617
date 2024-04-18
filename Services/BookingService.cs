using COM617.Data;
using COM617.Pages;
using COM617.Services.Identity;
using MongoDB.Driver;
using System.ComponentModel;

namespace COM617.Services
{
    public class BookingService
    {
        private readonly MongoDbService mongoDbService;
        private readonly UserState userState;

        public event EventHandler<Booking>? BookingChanged;
        public event EventHandler<Booking>? BookingAdded;
        public event EventHandler<Guid>? BookingDeleted;

        public IEnumerable<Booking> GetBookings() => 
            mongoDbService.GetDocumentsByFilter<Booking>(x => x.UserId == userState.CurrentUser!.Id);

        public IEnumerable<Booking> GetBookings(DateTime start, DateTime end)
        {
            var allBookings = GetBookings(); // Get all bookings for the current user

            // Filter the bookings where either the start or end time falls within the specified range
            var intersectingBookings = allBookings.Where(b =>
                (b.Start >= start && b.Start <= end) || (b.End >= start && b.End <= end));

            return intersectingBookings;
        }
        
        public Booking GetBooking(Guid id)
        {
            return mongoDbService.GetDocumentsByFilter<Booking>(x => x.Id == id).First();
        }

        public async Task<Booking> CreateBooking(DateTime start, DateTime end, Guid vehicleId, bool saveToDb = true)
        {
            var booking = new Booking
            {
                Start = start,
                End = end,
                UserId = userState.CurrentUser!.Id,
                VehicleId = vehicleId
            };
            if (saveToDb)
            {
                await mongoDbService.CreateDocument(booking);
                BookingAdded?.Invoke(this, booking);
            }

            return booking;
        }

        public async Task UpdateBooking(Booking booking)
        {
            var dbBooking = GetBooking(booking.Id);
            if (dbBooking is null)
                return;
            else
            {
                await mongoDbService.ReplaceDocument(booking.Id, booking);
                BookingChanged?.Invoke(this, booking);
            }
        }

        public async void CreateBooking(Booking booking) => await mongoDbService.CreateDocument(booking);

        public async void RemoveBooking(Guid bookingId)
        {
            await mongoDbService.DeleteDocument<Booking>(bookingId);
            BookingDeleted?.Invoke(this, bookingId);
        }

        public BookingService(MongoDbService mongoDbService, UserState userState)
        {
            this.mongoDbService = mongoDbService;
            this.userState = userState;
        }
    }
}
