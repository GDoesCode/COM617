using COM617.Data;
using COM617.Services.Identity;

namespace COM617.Services
{
    public class BookingService
    {
        private readonly MongoDbService mongoDbService;
        private readonly UserState userState;
        

        public async void AddBooking(DateTime start, DateTime end, Guid vehicleId)
        {
            var booking = new Booking
            {
                Start = start,
                End = end,
                UserId = userState.CurrentUser!.Id,
                VehicleId = vehicleId
            };

            await mongoDbService.CreateDocument(booking);
        }

        public async void RemoveBooking(Guid bookingId)
        {
            await mongoDbService.DeleteDocument<Booking>(bookingId);
        }

        public BookingService(MongoDbService mongoDbService, UserState userState)
        {
            this.mongoDbService = mongoDbService;
            this.userState = userState;
        }
    }
}
