using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Model
{
    public class ScrcpyEventArgs : EventArgs
    {
        public ScrcpyEventArgs(ProcessStartInfo processStartInfo)
        {
            ProcessStartInfo = processStartInfo;
        }

        public ProcessStartInfo ProcessStartInfo { get; set; }
    }
}
