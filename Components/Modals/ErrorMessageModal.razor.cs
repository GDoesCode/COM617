using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public class ErrorMessageInfo
    {
        public string Message { get; set; }
        public object Sender { get; set; }
        public object SenderData { get; set; }

        public ErrorMessageInfo(string message, object sender, object senderData) 
        { 
            Message = message;
            Sender = sender;
            SenderData = senderData;
        }
    }

    public partial class ErrorMessageModal
    {
        [Inject]
        private ModalService ModalService { get; set; } = null!;
        private ErrorMessageInfo DataIn { get; set; } = null!;

        protected override void OnInitialized()
        {
            DataIn = (ErrorMessageInfo)ModalService.DataIn!;
        }

        private void Close()
        {
            ModalService.Request(DataIn.Sender.GetType(), DataIn.SenderData);
        }
    }
}
