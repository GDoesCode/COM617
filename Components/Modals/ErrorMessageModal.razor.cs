using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public partial class ErrorMessageModal
    {
        [Inject]
        private ModalService ModalService { get; set; } = null!;

        private string message = null!;
        private object sendingModal = null!;
        private object dataIn = null!;

        protected override void OnInitialized()
        {
            (string, object, object) modalData = ((string, object, object))ModalService.DataIn!;
            message = modalData.Item1 as string;
            sendingModal = modalData.Item2;
            dataIn = modalData.Item3;
        }

        private void Close()
        {
            ModalService.Request(sendingModal.GetType(), dataIn);
        }
    }
}
