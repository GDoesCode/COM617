using Microsoft.AspNetCore.Components;

namespace COM617.Components.Nav
{
    public partial class NavItem
    {
        [Parameter]
        public string Link { get; set; } = string.Empty;

        [Parameter]
        public string Text { get; set; } = string.Empty;

        [Parameter]
        public required RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Active { get; set; } = false;

        [Parameter]
        public string CustomCSS { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<NavItem> OnClick { get; set; }

        private async Task Clicked() => await OnClick.InvokeAsync(this);

        public void SetActive(bool active) => Active = active;
    }
}
