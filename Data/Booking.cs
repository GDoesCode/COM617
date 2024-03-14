using COM617.Services;
using MongoDB.Bson.Serialization.Attributes;

namespace COM617.Data
{
    [MongoTypeMap("com617", "Bookings")]
    public class Booking
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Guid UserId { get; set; }
        public Guid VehicleId { get; set; }

        public Booking() { }

        public Booking(Guid id, DateTime start, DateTime end, Guid userId)
        {
            Id = id;
            Start = start;
            End = end;
            UserId = userId;
        }
    }
}
