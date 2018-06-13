using System.Diagnostics;

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
