namespace scrcpy.VisualStudio.Model
{
    /// <summary>
    /// Represents an Android device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="id">The device ID.</param>
        /// <param name="description">The device description.</param>
        public Device(string id, string description)
        {
            ID = id;
            Description = description;
        }

        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Gets the device description.
        /// </summary>
        public string Description { get; }
    }
}
