using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Android
{
    public static class ScrcpyWrapper
    {
        public static ProcessStartInfo GetStartInfo(string device)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                WorkingDirectory = Paths.ScrcpyPath,
                FileName = Paths.ScrcpyNoConsoleName,
                Arguments = $"-s {device}",
            };
            return psi;
        }
    }
}
