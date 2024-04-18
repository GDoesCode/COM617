using COM617.Components.Modals;
using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Pages
{
    public partial class UserBooking : IDisposable
    {
        [Inject]
        private VehicleService VehicleService { get; set; } = null!;

        [Inject]
        private ModalService ModalService { get; set; } = null!;

        [Inject]
        private BookingService BookingService { get; set; } = null!;

        private List<Booking> bookings = null!;

        protected override void OnInitialized()
        {
            bookings = BookingService!.GetBookings().ToList();
            StateHasChanged();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                BookingService.BookingChanged += BookingChanged;
                BookingService.BookingAdded += BookingAdded;
                BookingService.BookingDeleted += BookingRemoved;
            }
                
        }

        private void BookingAdded(object? _, Booking booking)
        {
            bookings.Add(booking);
            StateHasChanged();
        }

        private void BookingRemoved(object? _, Guid bookingId)
        {
            bookings.RemoveAll(x => x.Id == bookingId);
            StateHasChanged();
        }

        private void BookingChanged(object? _, Booking booking)
        {
            foreach (var b in bookings)
            {
                if (booking.Id == b.Id)
                {
                    bookings.Remove(b);
                    bookings.Add(booking);
                    break;
                }
            }
            StateHasChanged();
        }

        public void Dispose()
        {
            BookingService.BookingChanged -= BookingChanged;
            BookingService.BookingDeleted -= BookingRemoved;
            BookingService.BookingAdded -= BookingAdded;
        }
    }
}
