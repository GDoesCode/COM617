using COM617.Components.Modals;
using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class BookVehicle
    {
        private bool isOpen;
        private List<Vehicle> vehicles => GetVehicles();

        [Inject]
        private VehicleService VehicleService { get; set; } = null!;

        protected override void OnInitialized()
        {
            VehicleService.VehiclesChanged += (_, _) => InvokeAsync(StateHasChanged);
        }

        private List<Vehicle> GetVehicles()
        {
            return mongoDbService.GetDocumentsByFilter<Vehicle>(vehicle => vehicle.Id != Guid.Empty).ToList();
        }

        private void EditVehicle(Vehicle vehicle)
        {
            modalService.Request(typeof(VehicleModal), vehicle);
            modalService.DataOutChanged += (_, _) => StateHasChanged();
        }

        private void CreateVehicle()
        {
            modalService.Request(typeof(VehicleModal), null);
            modalService.DataOutChanged += (_, _) => StateHasChanged();
        }

        private async Task DeleteVehicle(Vehicle vehicle)
        {
            await mongoDbService.DeleteDocument<Vehicle>(vehicle.Id);
            StateHasChanged();
        }
    }
}
