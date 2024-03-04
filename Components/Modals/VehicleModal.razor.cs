using COM617.Data;
using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public partial class VehicleModal
    {
        private Vehicle vehicle = new();

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

        private Vehicle Vehicle
        {
            get
            {
                if (ModalService!.DataIn is null) // Creating new Vehicle
                    return vehicle;
                else
                    return (Vehicle) ModalService!.DataIn!; // Editing current vehicle
            }
        }

        private void Create()
        {
            if (Vehicle!.Type == VehicleType.Undefined)
                ShowError(VehicleFormError.TypeInvalid);
            else if (Vehicle!.Year < 1900)
                ShowError(VehicleFormError.YearInvalid);
            else if (Vehicle!.Manufacturer == string.Empty)
                ShowError(VehicleFormError.MakeInvalid);
            else if (Vehicle!.Seats == 0)
                ShowError(VehicleFormError.SeatsInvalid);
            else if (Vehicle!.Doors == 0)
                ShowError(VehicleFormError.DoorsInvalid);

            ModalService!.Finish(vehicle);
        }

        private void ShowError(VehicleFormError error)
        {
            ModalService!.Request(typeof(ErrorMessageModal), (VehicleFormErrorMessage[error], this, vehicle));
        }

        private Dictionary<VehicleType, string> GetVehicleTypeOptions()
        {
            Dictionary<VehicleType, string> options = new Dictionary<VehicleType, string>();
            foreach (var vehicleType in Enum.GetValues<VehicleType>().ToList())
            {

            }
            
            return options;
        }
    }
}
