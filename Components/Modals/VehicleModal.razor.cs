﻿using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;

namespace COM617.Components.Modals
{
    public partial class VehicleModal
    {
        private Vehicle vehicle = null!;
        private readonly PropertyInfo[] properties = typeof(Vehicle).GetProperties();

        private enum VehicleFormError
        {
            None,
            ModelNameInvalid,
            MakeInvalid,
            RegistrationInvalid,
            TypeInvalid,
            YearInvalid,
            SeatsInvalid,
            DoorsInvalid
        }

        private Dictionary<VehicleFormError, string> VehicleFormErrorMessage = new()
        {
            { VehicleFormError.None, ""},
            { VehicleFormError.DoorsInvalid, "The vehicle must have at least one door." },
            { VehicleFormError.TypeInvalid, "The vehicle must have a type." }

        };

        [Inject] 
        private ModalService? ModalService { get; set; }

        /// <summary>
        /// If true, creating new Vehicle. If false, editing current vehicle.
        /// </summary>
        private bool Mode => ModalService!.DataIn is null;

        protected override void OnInitialized()
        {
            if (ModalService!.DataIn is null) // Creating new Vehicle
                vehicle = new();
            else
                vehicle = (Vehicle)ModalService!.DataIn!; // Editing current vehicle
        }

        private void ChangeValue(string propertyName, object value)
        {
            properties.First(p => p.Name == propertyName).SetValue(vehicle, value);
            StateHasChanged();
        }

        private async Task Delete()
        {
            await VehicleService.DeleteVehicle(vehicle);
            ModalService!.Close();
        }


        private async Task Create()
        {
            if (vehicle!.Type == VehicleType.Undefined)
                ShowError(VehicleFormError.TypeInvalid);
            else if (vehicle!.Year < 1900)
                ShowError(VehicleFormError.YearInvalid);
            else if (vehicle!.Manufacturer == string.Empty)
                ShowError(VehicleFormError.MakeInvalid);
            else if (vehicle!.Seats == 0)
                ShowError(VehicleFormError.SeatsInvalid);
            else if (vehicle!.Doors == 0)
                ShowError(VehicleFormError.DoorsInvalid);
            else
            {
                if (ModalService!.DataIn is null)
                    await VehicleService.CreateVehicle(vehicle);
                else
                    await VehicleService.UpdateVehicle(vehicle);
                ModalService!.Finish(vehicle);
            }
                
        }

        private async Task HandleSelectedFile(InputFileChangeEventArgs args)
        {
            var file = args.File;
            long maxAllowedSize = 10 * 1024 * 1024; // Maximum file size (10 MB)
            if (file != null)
            {
                try
                {
                    var format = "image/png";
                    var resizedImage = await file.RequestImageFileAsync(format, 640, 480); // Optional: Resize image

                    using var ms = new MemoryStream();
                    await resizedImage.OpenReadStream(maxAllowedSize).CopyToAsync(ms);
                    vehicle.ImageSrc = $"data:{format};base64,{Convert.ToBase64String(ms.ToArray())}";
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading image: {ex.Message}");
                }
            }
        }

        private void ShowError(VehicleFormError error)
        {
            ModalService!.Request(typeof(ErrorMessageModal), new ErrorMessageInfo(VehicleFormErrorMessage[error], this, vehicle));
        }
    }
}
