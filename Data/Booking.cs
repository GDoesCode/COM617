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
        public double StartLatitude { get; set; } // Latitude of start location
        public double StartLongitude { get; set; } // Longitude of start location
        public double EndLatitude { get; set; } // Latitude of end location
        public double EndLongitude { get; set; } // Longitude of end location
        public double Distance { get; set; } // Calculated distance between locations

        public Booking() { }

        public Booking(Guid id, DateTime start, DateTime end, Guid userId, double startLat, double startLong, double endLat, double endLong, double distance)
        {
            Id = id;
            Start = start;
            End = end;
            UserId = userId;
            StartLatitude = startLat;
            StartLongitude = startLong;
            EndLatitude = endLat;
            EndLongitude = endLong;
            Distance = distance;
        }
    }

}
