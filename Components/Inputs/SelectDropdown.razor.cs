using Microsoft.AspNetCore.Components;

namespace COM617.Components.Inputs
{
    public partial class SelectDropdown<T>
    {
        private bool isOpen;
        private bool highlighted;
        private bool wasSelected;
        private T? selected;

        /// <summary>
        /// The objects to use as options in the select drop-down.
        /// </summary>
        [Parameter]
        public required List<T> Options { get; set; }

        /// <summary>
        /// The option object that is currently selected.
        /// </summary>
        [Parameter]
        public T? Selection { get; set; }

        /// <summary>
        /// If <see langword="true" />, the old selection value will persist when <see cref="OnParametersSet"/> is called,
        /// otherwise, the selection displayed will revert to the original value.
        /// </summary>
        [Parameter]
        public bool IsPersistent { get; set; }

        /// <summary>
        /// The option to display to the user when representing a null value.
        /// </summary>
        /// <remarks>Leaving this property as null results in a forced selection by the user.</remarks>
        [Parameter]
        public string? NullValue { get; set; }

        /// <summary>
        /// The option to display to the user as a placeholder.
        /// </summary>
        /// <remarks>The placeholder represents null and will not reappear once another option is selected.</remarks>
        [Parameter]
        public string? Placeholder { get; set; }

        /// <summary>
        /// The EventCallback that fires when the Selection has been changed.
        /// </summary>
        [Parameter]
        public EventCallback<T> SelectionChanged { get; set; }

        /// <summary>
        /// Content to be displayed on the dropdown button.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        private void Select(T? value)
        {
            wasSelected = true;
            selected = value;
            SelectionChanged.InvokeAsync(value!);
        }

        protected override void OnInitialized()
        {
            selected = Selection;
        }

        protected override void OnParametersSet()
        {
            if (!wasSelected && !IsPersistent)
                selected = Selection;
            wasSelected = false;
        }

        /// <summary>
        /// Rerenders the Simpledropdown.
        /// </summary>
        public void Reset()
        {
            StateHasChanged();
        }
    }
}
