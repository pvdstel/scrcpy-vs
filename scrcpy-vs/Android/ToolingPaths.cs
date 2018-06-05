using System.IO;
using System.Reflection;

namespace scrcpy.VisualStudio.Android
{
    /// <summary>
    /// Maintains paths for scrcpy.
    /// </summary>
    internal static class ToolingPaths
    {
        /// <summary>
        /// The name of the ADB executable.
        /// </summary>
        private const string AdbName = "adb.exe";

        /// <summary>
        /// The name of the scrcpy executable.
        /// </summary>
        private const string ScrcpyNoConsoleName = "scrcpy-noconsole.exe";

        /// <summary>
        /// The name of the directory containing android executables.
        /// </summary>
        private const string ToolingDirectory = "scrcpy-win64";

        /// <summary>
        /// Gets the path of the directory containing Android tools.
        /// </summary>
        public static string Root { get; }

        /// <summary>
        /// Gets the path to the adb executable.
        /// </summary>
        public static string AdbPath { get; }

        /// <summary>
        /// Gets the path to the scrcpy executable.
        /// </summary>
        public static string ScrcpyPath { get; }

        /// <summary>
        /// Initializes static members of <see cref="ToolingPaths"/>.
        /// </summary>
        static ToolingPaths()
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Root = Path.Combine(exeDirectory, ToolingDirectory);

            AdbPath = Path.Combine(Root, AdbName);
            ScrcpyPath = Path.Combine(Root, ScrcpyNoConsoleName);
        }
    }
}
