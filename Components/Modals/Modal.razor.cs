using COM617.Services;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Modals
{
    public partial class Modal
    {
        /// <summary>
        /// The content of the modal.
        /// </summary>
        /// <remarks>This parameter is mandatory.</remarks>
        [Parameter]
        public required RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Custom styling to apply to the modal.
        /// </summary>
        [Parameter]
        public string CustomCSS { get; set; } = string.Empty;

        [Inject]
        private ModalService? ModalService { get; set; }
    }
}
