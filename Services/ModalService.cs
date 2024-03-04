namespace COM617.Services
{
    public sealed class ModalService
    {
        /// <summary>
        /// Fires when the modal state changes.
        /// </summary>
        public event EventHandler? ModalUpdated;

        /// <summary>
        /// Fires when <see cref="Finish"/> is called. Use this to notify listeners that data processing is complete.
        /// </summary>
        public event EventHandler? DataOutChanged;

        /// <summary>
        /// Gets a value indicating whether the modal is showing.
        /// </summary>
        public bool ModalShowing { get; private set; }

        /// <summary>
        /// Gets the type of the modal.
        /// </summary>
        public Type? ModalType { get; private set; }

        /// <summary>
        /// Gets the data in.
        /// </summary>
        public object? DataIn { get; private set; }

        /// <summary>
        /// Gets the data out.
        /// </summary>
        public object? DataOut { get; private set; }

        /// <summary>
        /// Used for notifying listeners that the modal has finished processing data,
        /// and stores the data in <see cref="DataOut"/>.
        /// </summary>
        public void Finish(object data)
        {
            DataOut = data;
            Close();
            DataOutChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the modal, resets all parameter values.
        /// </summary>
        public void Close()
        {
            ModalType = null;
            ModalShowing = false;
            DataIn = null;
            ModalUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Requests the modal to be displayed.
        /// </summary>
        /// <param name="type">The type of modal being requested.</param>
        /// <param name="data">The data to pass to the modal.</param>
        public void Request(Type type, object? data)
        {
            DataOut = null;
            DataIn = data;
            ModalType = type;
            ModalShowing = true;
            ModalUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
