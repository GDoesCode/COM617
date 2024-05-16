using COM617.Components.Modals;
using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class BookingManagement
    {
        private List<Booking> bookings = new List<Booking>();

        [Inject]
        private BookingService BookingService { get; set; } = null!;

        [Inject]
        private ModalService ModalService { get; set; } = null!;

        [Inject]
        private VehicleService VehicleService { get; set; } = null!;

        [Inject]
        private UserService UserService { get; set; } = null!;

        protected override void OnInitialized()
        {
            bookings = BookingService.GetCurrentUserBookings().ToList();
        }
    }
}
