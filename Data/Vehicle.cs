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
        public bool Booked { get; set; } = false;

        public Vehicle(string make, string modelName, string modelNumber, string plate, string color, VehicleType type, int year, int doors, int seats)
        {
            Id = Guid.NewGuid();
            Make = make;
            ModelName = modelName;
            ModelNumber = modelNumber;
            Plate = plate;
            Color = Color;
            Type = type;
            Year = year;
            Doors = doors;
            Seats = seats;
        }
    }
}
