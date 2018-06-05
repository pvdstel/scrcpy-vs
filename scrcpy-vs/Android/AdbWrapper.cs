using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Android
{
    /// <summary>
    /// Wraps ADB functionality.
    /// </summary>
    public static class AdbWrapper
    {
        /// <summary>
        /// Gets connected and authorized devices.
        /// </summary>
        /// <returns>A list of device IDs.</returns>
        public static async Task<List<string>> GetAuthorizedDevicesAsync()
        {
            string output = await GetAdbOutputAsync("devices");

            return output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(s => s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(d => d[1] == "device")
                .Select(d => d[0])
                .ToList();
        }

        /// <summary>
        /// Gets the device manufacturer.
        /// </summary>
        /// <param name="device">The device ID.</param>
        /// <returns>The device manufacturer.</returns>
        public static async Task<string> GetDeviceManufacturerAsync(string device)
        {
            string output = await GetAdbOutputAsync($"-s {device} shell getprop ro.product.manufacturer");
            return output.Trim();
        }

        /// <summary>
        /// Gets the device model.
        /// </summary>
        /// <param name="device">The device ID.</param>
        /// <returns>The device model.</returns>
        public async static Task<string> GetDeviceModelAsync(string device)
        {
            string output = await GetAdbOutputAsync($"-s {device} shell getprop ro.product.model");
            return output.Trim();
        }

        /// <summary>
        /// Kills the ADB server.
        /// </summary>
        public static void KillServer()
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                WorkingDirectory = ToolingPaths.Root,
                FileName = ToolingPaths.AdbPath,
                Arguments = "kill-server",
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process adb = Process.Start(psi);
            adb.WaitForExit();
        }

        /// <summary>
        /// Runs ADB with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments to run against ADB.</param>
        /// <returns>The output of ADB.</returns>
        private static Task<string> GetAdbOutputAsync(string arguments)
        {
            return Task.Run(async () =>
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    WorkingDirectory = ToolingPaths.Root,
                    FileName = ToolingPaths.AdbPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                Process adb = Process.Start(psi);
                adb.WaitForExit();
                string output = await adb.StandardOutput.ReadToEndAsync();
                return output;
            });
        }
    }
}
