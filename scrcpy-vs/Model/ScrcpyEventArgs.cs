using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Model
{
    public class ScrcpyEventArgs : EventArgs
    {
        public ScrcpyEventArgs(Device device)
        {
            Device = device;
        }

        public Device Device { get; set; }
    }
}
