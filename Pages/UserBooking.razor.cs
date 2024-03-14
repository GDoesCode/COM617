using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class UserBooking
    {
        [Inject]
        private VehicleService? VehicleService {  get; set; }

        private List<Vehicle> vehicles { get; set; } = new();

        private void FilterVehicles()
        {
            vehicles = VehicleService!.GetVehicles();
        }

        private void InputChanged(string input)
        {
            vehicles = VehicleService!.GetVehicles().Where(vehicle => vehicle.ModelName.Contains(input)).ToList();
            StateHasChanged();
        }

        private void CreateBooking(Vehicle vehicle)
        {

        }

        protected override void OnInitialized()
        {
            FilterVehicles();
            StateHasChanged();
        }
    }
}
