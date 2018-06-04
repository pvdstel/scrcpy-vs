using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Model
{
    /// <summary>
    /// Represents an Android device.
    /// </summary>
    public class Device
    {
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
