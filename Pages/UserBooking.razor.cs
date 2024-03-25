using COM617.Components.Inputs;
using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class UserBooking
    {
        [Inject]
        private VehicleService? VehicleService { get; set; }

        private List<Vehicle> Vehicles { get; set; } = new();

        private SelectDropdown<VehicleType> Type { get; set; } = ; // Reference to the object.

        private void FilterVehicles()
        {
            Vehicles = VehicleService!.GetVehicles();
        }

        private void InputChanged(string input)
        {
            VehicleType type = Type.Selection;
            Vehicles = VehicleService!.GetVehicles().Where(vehicle => vehicle.ModelName.Contains(input) && vehicle.Type == type).ToList();
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
