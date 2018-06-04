using System.IO;
using System.Reflection;

namespace scrcpy.VisualStudio.Android
{
    internal static class Paths
    {
        private const string ScrcpyDirectory = "scrcpy-win64";
        public const string AdbName = "adb.exe";
        public const string ScrcpyNoConsoleName = "scrcpy-noconsole.exe";

        public static string ScrcpyPath { get; }
        
        static Paths()
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ScrcpyPath = Path.Combine(exeDirectory, ScrcpyDirectory);
        }
    }
}
