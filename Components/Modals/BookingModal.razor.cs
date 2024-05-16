using COM617.Data;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace COM617.Components.Modals
{
    public partial class BookingModal
    {
        private enum Mode
        {
            DataEntry,
            StartLocation,
            EndLocation,
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

        [Inject]
        private UserState UserState { get; set; } = null!;

        [Inject]
        private IJSRuntime JsRuntime { get; set; } = null!;

        private DateOnly startDate;
        private DateOnly endDate;
        private TimeOnly startTime;
        private TimeOnly endTime;
        private double startLatitude;
        private double startLongitude;
        private double endLatitude;
        private double endLongitude;
        private double distance;
        private string startAddress = string.Empty;
        private string endAddress = string.Empty;
        private DateTime StartDateTime => new(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
        private DateTime EndDateTime => new(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
        private Mode mode;
        private List<Vehicle> availableVehicles = new();
        private Booking booking = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var objRef = DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("initializeBingMaps", objRef);
        }

        protected override void OnInitialized()
        {
            booking.UserId = UserState.CurrentUser!.Id;
            if (ModalService.DataIn is null)
                mode = Mode.DataEntry;
            else
            {
                booking = (Booking) ModalService.DataIn;
                mode = Mode.ModifyExisting;
            }
        }

        [JSInvokable("OnMapClicked")]
        public async void OnMapClicked(double latitude, double longitude)
        {
            if (mode == Mode.StartLocation)
            {
                startLatitude = latitude;
                startLongitude = longitude;
                startAddress = await GetAddress(latitude, longitude);
            }
            else if (mode == Mode.EndLocation)
            {
                endLatitude = latitude;
                endLongitude = longitude;
                endAddress = await GetAddress(latitude, longitude);
            }
            if (startLatitude != 0 && endLatitude != 0 && startLongitude != 0 && endLongitude != 0)
            {
                distance = await CalculateDistance();
            }
            await InvokeAsync(StateHasChanged);
        }

        private void ConfirmDateTime()
        {
            booking.Start = StartDateTime;
            booking.End = EndDateTime;
            mode = Mode.StartLocation;
            StateHasChanged();
        }

        private void ConfirmLocation()
        {
            booking.StartLatitude = startLatitude;
            booking.StartLongitude = startLongitude;
            booking.EndLatitude = endLatitude;
            booking.EndLongitude = endLongitude;
            booking.Distance = distance;
            availableVehicles.AddRange(VehicleService.GetAvailableVehicles(StartDateTime, EndDateTime));
            mode = Mode.VehicleSelection;
            StateHasChanged();
        }

        private async Task SelectVehicle(Vehicle vehicle)
        {
            booking.VehicleId = vehicle.Id;
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

        private async Task<double> CalculateDistance()
        {
            using var client = new HttpClient();
            string apiKey = "ApXUlPc2Bp6Pm9sbrCr5URzTiJxpa3ORFy-jrPgtecmQXCkGOcZ7i820Lp97gpNr";

            var response = await client.GetAsync($"https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins={startLatitude},{startLongitude}&destinations={endLatitude},{endLongitude}&travelMode=driving&distanceUnit=mi&key={apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<BingMapsDistanceMatrixResponse>(content);

                // Extract distance from result and return it
                var distance = result?.resourceSets?[0]?.resources?[0]?.results?[0]?.travelDistance ?? 0;
                return distance;
            }
            else
            {
                // Handle API error
                return 0;
            }
        }

        private async Task<string> GetAddress(double lat, double lo)
        {
            using var client = new HttpClient();
            string apiKey = "ApXUlPc2Bp6Pm9sbrCr5URzTiJxpa3ORFy-jrPgtecmQXCkGOcZ7i820Lp97gpNr";

            // Reverse geocoding to retrieve location information
            var reverseGeocodeResponse = await client.GetAsync($"https://dev.virtualearth.net/REST/v1/Locations/{lat},{lo}?includeEntityTypes=Address&key={apiKey}");

            if (reverseGeocodeResponse.IsSuccessStatusCode)
            {
                var reverseGeocodeContent = await reverseGeocodeResponse.Content.ReadAsStringAsync();
                var reverseGeocodeResult = JsonSerializer.Deserialize<BingMapsReverseGeocodeResponse>(reverseGeocodeContent);
                string address;
                try
                {
                    address = reverseGeocodeResult?.resourceSets?[0]?.resources?[0]?.address?.formattedAddress ?? "";
                } 
                catch(Exception ex)
                {
                    return string.Empty;
                }
                

                return address;
            }
            else
            {
                return string.Empty;
            }
        }

        public class BingMapsDistanceMatrixResponse
        {
            public BingMapsResourceSet[] resourceSets { get; set; }
        }

        public class BingMapsReverseGeocodeResponse
        {
            public BingMapsResourceSet[] resourceSets { get; set; }
        }

        public class BingMapsResourceSet
        {
            public BingMapsResource[] resources { get; set; }
        }

        public class BingMapsResource
        {
            public BingMapsDistanceMatrixResult[] results { get; set; }
            public BingMapsAddress address { get; set; }
        }

        public class BingMapsAddress
        {
            public string formattedAddress { get; set; }
        }

        public class BingMapsDistanceMatrixResult
        {
            public double travelDistance { get; set; } // Distance in meters
        }
    }
}
