using COM617.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;

namespace COM617.Services
{
    public class VehicleService
    {
        private List<Vehicle> vehicles;
        private MongoDbService mongoDbService;

        public VehicleService(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
            vehicles = mongoDbService.GetQueryableCollection<Vehicle>().ToList();
        }

        public IEnumerable<string> GetVehicleManufacturers(string modelName = "")
        {
            var manufacturers = new List<string>();
            if (modelName == "")
                foreach (var vehicle in vehicles)
                    if (!manufacturers.Contains(vehicle.Manufacturer))
                        manufacturers.Add(vehicle.Manufacturer);
            else
                foreach (var vehicleM in vehicles)
                    if (!manufacturers.Contains(vehicleM.Manufacturer))
                        manufacturers.Add(vehicleM.Manufacturer);
            return manufacturers;
        }

        public IEnumerable<string> GetVehicleModels(string manufacturer = "")
        {
            var models = new List<string>();
            if (manufacturer == "")
                foreach (var vehicle in vehicles)
                    if (!models.Contains(vehicle.ModelName))
                        models.Add(vehicle.ModelName);
            else
                foreach (var vehicleM in vehicles.Where(v => v.Manufacturer == manufacturer))
                    if (!models.Contains(vehicleM.ModelName))
                        models.Add(vehicleM.ModelName);
            return models;
        }
    }
}
