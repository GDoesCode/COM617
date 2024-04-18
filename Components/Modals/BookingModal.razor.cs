using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public partial class BookingModal
    {
        private enum Mode
        {
            DataEntry,
            VehicleSelection,
            Confirmation,
            ModifyExisting
        }

        [Inject]
        private VehicleService VehicleService { get; set; } = null!;

        [Inject]
        private ModalService ModalService { get; set; } = null!;

        [Inject]
        private BookingService BookingService { get; set; } = null!;

        private DateOnly startDate;
        private DateOnly endDate;
        private TimeOnly startTime;
        private TimeOnly endTime;
        private DateTime StartDateTime => new(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
        private DateTime EndDateTime => new(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
        private Mode mode;
        private List<Vehicle> availableVehicles = new();
        private Booking booking = null!;

        protected override void OnInitialized()
        {
            if (ModalService.DataIn is null)
                mode = Mode.DataEntry;
            else
            {
                booking = (Booking) ModalService.DataIn;
                mode = Mode.ModifyExisting;
            }
        }

        private void Search()
        {
            availableVehicles.AddRange(VehicleService.GetAvailableVehicles(StartDateTime, EndDateTime));
            mode = Mode.VehicleSelection;
            StateHasChanged();
        }

        private async Task SelectVehicle(Vehicle vehicle)
        {
            booking = await BookingService.CreateBooking(StartDateTime, EndDateTime, vehicle.Id, false);
            mode = Mode.Confirmation;
            await InvokeAsync(StateHasChanged);
        }

        private void ConfirmBooking()
        {
            BookingService.CreateBooking(booking);
            ModalService.Finish(booking);
        }

        private async Task ModifyBooking()
        {
            await BookingService.UpdateBooking(booking);
            ModalService.Finish(booking);
        }

        private void DeleteBooking()
        {
            BookingService.RemoveBooking(booking.Id);
            ModalService.Finish(booking);
        }
    }
}
