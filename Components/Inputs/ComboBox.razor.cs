using COM617.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.JSInterop;

namespace COM617.Components.Inputs
{
    public partial class ComboBox<T>
    {
        private string input = string.Empty;
        private string Input { get { return input; } set => SetInput(value); }
        private bool comboVisible;
        private string uniqueId = Guid.NewGuid().ToString();
        private IEnumerable<T> FilteredOptions => Options.Where(Filter!);

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = null!;

        [Parameter]
        public Func<T, bool>? Filter { get; set; } 

        [Parameter]
        public required IEnumerable<T> Options { get; set; }

        [Parameter]
        public T? Selection { get; set; }

        [Parameter]
        public EventHandler<string>? InputChanged { get; set; }

        protected override void OnInitialized()
        {
            Filter ??= x => x!.ToString()!.Contains(input);
            input = Selection?.ToString()!;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            var objRef = DotNetObjectReference.Create(this);

            if (firstRender)
                await JSRuntime!.InvokeVoidAsync("Dropdown.register", $"#combobox-{uniqueId}", $"#options-{uniqueId}", objRef);
        }

        private void SetInput(string value)
        {
            input = value;
            StateHasChanged();
            InputChanged?.Invoke(this, input);
        }

        [JSInvokable("OnCloseEventReceived")]
        public void OnCloseEventReceived()
        {
            comboVisible = false;
            StateHasChanged();
        }
    }
}
