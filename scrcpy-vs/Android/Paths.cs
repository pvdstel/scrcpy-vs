using System.IO;
using System.Reflection;

namespace scrcpy.VisualStudio.Android
{
    /// <summary>
    /// Maintains paths for scrcpy.
    /// </summary>
    internal static class Paths
    {
        /// <summary>
        /// The name of the ADB executable.
        /// </summary>
        public const string AdbName = "adb.exe";
        /// <summary>
        /// The name of the scrcpy executable.
        /// </summary>
        public const string ScrcpyNoConsoleName = "scrcpy-noconsole.exe";

        private const string ScrcpyDirectory = "scrcpy-win64";

        /// <summary>
        /// Gets the path of the directory containing scrcpy.
        /// </summary>
        public static string ScrcpyPath { get; }
        
        /// <summary>
        /// Initializes static members of <see cref="Paths"/>.
        /// </summary>
        static Paths()
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ScrcpyPath = Path.Combine(exeDirectory, ScrcpyDirectory);
        }
    }
}
