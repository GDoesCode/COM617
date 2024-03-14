using COM617.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using System.ComponentModel;

namespace COM617.Services
{
    public class VehicleService
    {
        private List<Vehicle> vehicles;
        private MongoDbService mongoDbService;

        public event EventHandler? VehiclesChanged;

        public VehicleService(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
            vehicles = mongoDbService.GetQueryableCollection<Vehicle>().ToList();
        }

        public IEnumerable<string> GetVehicleManufacturers(string modelName = "")
        {
            var manufacturers = new List<string>();
            if (modelName == "")
            {
                foreach (var vehicle in vehicles)
                    if (!manufacturers.Contains(vehicle.Manufacturer))
                        manufacturers.Add(vehicle.Manufacturer);
            }
            else
            {
                foreach (var vehicleM in vehicles.Where(v => v.ModelName.ToLower() == modelName.ToLower()))
                    if (!manufacturers.Contains(vehicleM.Manufacturer))
                        manufacturers.Add(vehicleM.Manufacturer);
            }
                
            return manufacturers;
        }

        public IEnumerable<string> GetVehicleModels(string manufacturer = "")
        {
            var models = new List<string>();
            if (manufacturer == "")
            {
                foreach (var vehicle in vehicles)
                    if (!models.Contains(vehicle.ModelName))
                        models.Add(vehicle.ModelName);
            }
            else
            {
                foreach (var vehicleM in vehicles.Where(v => v.Manufacturer.ToLower() == manufacturer.ToLower()))
                    if (!models.Contains(vehicleM.ModelName))
                        models.Add(vehicleM.ModelName);
            }
                
            return models;
        }

        public async Task UpdateVehicle(Vehicle vehicle)
        {
            await mongoDbService.ReplaceDocument(vehicle.Id, vehicle);
            VehiclesChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task CreateVehicle(Vehicle vehicle)
        {
            await mongoDbService.CreateDocument(vehicle);
            vehicles.Add(vehicle);
            VehiclesChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteVehicle(Vehicle vehicle)
        {
            await mongoDbService.DeleteDocument<Vehicle>(vehicle.Id);
            vehicles.Remove(vehicle);
            VehiclesChanged?.Invoke(this, EventArgs.Empty);
        }

        public List<Vehicle> GetVehicles()
        {
            return mongoDbService.GetQueryableCollection<Vehicle>().ToList();
        }
    }
}
