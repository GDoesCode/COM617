using COM617.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace COM617.Data
{
    public class VehicleTypeImageSrcAttribute : Attribute
    {
        public string ImageSrc { get; set; }
        public VehicleTypeImageSrcAttribute(string imgSrc) => ImageSrc = imgSrc;
    }

    public enum VehicleType
    {
        [VehicleTypeImageSrc("default.jpg")]
        Undefined,
        [VehicleTypeImageSrc("")]
        Car,
        [VehicleTypeImageSrc("")]
        CarXL,
        [VehicleTypeImageSrc("")]
        Van,
        [VehicleTypeImageSrc("")]
        Truck
    }

    [MongoTypeMap("com617", "Vehicles")]
    public class Vehicle
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ImageSrc { get; set; } = string.Empty;
        public VehicleType Type { get; set; }
        public int Year { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }

        public Vehicle()
        {
            Id = Guid.NewGuid();
        }
    }
}
