using COM617.Data;

namespace COM617.Services.Identity
{
    /// <summary>
    /// Allows a user to book a vehicle
    /// </summary>
    public class VehicleBookingService
    {
        /*
         * A standard user should be able to book a vehicle for the duration making it unavailable for others during this time.
         * This will also log the trip so the millage can be tracked by the accounts team.
         */

        private readonly MongoDbService mongoDbService;

        public VehicleBookingService(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
        }

        public void BookVehicle(User user, Vehicle vehicle) // string startDate, string endDate
        {
            vehicle.Booked = true;
        }
    }
}
