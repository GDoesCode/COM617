using COM617.Shared;
using Microsoft.AspNetCore.Components;

namespace COM617.Components.Nav
{
    public partial class TopNavbar
    {

        [Parameter]
        public EventHandler? OnClick { get; set; }

        private void Clicked() => OnClick?.Invoke(this, EventArgs.Empty);
    }
}
