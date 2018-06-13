using System;
using System.Diagnostics;

namespace scrcpy.VisualStudio.Model
{
    /// <summary>
    /// Represents event arguments for a Start event.
    /// </summary>
    public class ScrcpyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrcpyEventArgs"/> class.
        /// </summary>
        /// <param name="processStartInfo">The process start information.</param>
        public ScrcpyEventArgs(ProcessStartInfo processStartInfo)
        {
            ProcessStartInfo = processStartInfo;
        }

        /// <summary>
        /// Gets or sets the process start information.
        /// </summary>
        public ProcessStartInfo ProcessStartInfo { get; set; }
    }
}
