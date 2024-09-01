
namespace ConfigurationStorage
{
    /// <summary>
    /// Represents the arguments for an event that is triggered when data changes.
    /// </summary>
    public class DataChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        public string PropertyName { get; init; }

        /// <summary>
        /// Gets the old value of the property.
        /// </summary>
        public object OldValue { get; init; }

        /// <summary>
        /// Gets the new value of the property.
        /// </summary>
        public object NewValue { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        public DataChangedEventArgs(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
