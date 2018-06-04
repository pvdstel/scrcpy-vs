using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrcpy.VisualStudio.Native
{
    internal class Constants
    {
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_HWNDPARENT = -8;

        public const int WS_VISIBLE = 0x10000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_TABSTOP = 0x00010000;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_MDICHILD = 0x00000040;
    }
}
