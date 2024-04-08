using Microsoft.AspNetCore.Components;

namespace COM617.Components.Inputs
{
    public partial class IconDropdown<T>
    {
        private T? Value { get; set; }

        [Parameter]
        public Dictionary<T, string> Options { get; set; } = null!;

        [Parameter]
        public T? Selected {  get; set; }

        protected override void OnInitialized()
        {
            if (Selected  != null)
            {
                Value = Selected;
                StateHasChanged();
            }
        }


    }
}
