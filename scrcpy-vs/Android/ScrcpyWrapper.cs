using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Android
{
    /// <summary>
    /// Wraps scrcpy functionality.
    /// </summary>
    public static class ScrcpyWrapper
    {
        /// <summary>
        /// Gets scrcpy process start information for the specified device.
        /// </summary>
        /// <param name="device">The device ID.</param>
        /// <returns>Relevant process start information.</returns>
        public static ProcessStartInfo GetStartInfo(string device)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                WorkingDirectory = ToolingPaths.Root,
                FileName = ToolingPaths.ScrcpyPath,
                Arguments = $"-s {device}",
            };
            return psi;
        }
    }
}
