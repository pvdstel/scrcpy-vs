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
    public static class Adb
    {
        public static async Task<List<string>> GetAuthorizedDevices()
        {
            string output = await GetAdbOutput("devices");

            return output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(s => s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(d => d[1] == "device")
                .Select(d => d[0])
                .ToList();
        }

        public static async Task<string> GetDeviceManufacturer(string device)
        {
            string output = await GetAdbOutput($"-s {device} shell getprop ro.product.manufacturer");
            return output.Trim();
        }

        public async static Task<string> GetDeviceModel(string device)
        {
            string output = await GetAdbOutput($"-s {device} shell getprop ro.product.model");
            return output.Trim();
        }

        private static Task<string> GetAdbOutput(string arguments)
        {
            return Task.Run(async () =>
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    WorkingDirectory = Paths.ScrcpyPath,
                    FileName = Paths.AdbName,
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
