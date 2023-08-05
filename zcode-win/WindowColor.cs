using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowColor : IColor
    {
        public System.Drawing.Color WindowsColor { get;  }
        public WindowColor(System.Drawing.Color windowsColor) {  WindowsColor = windowsColor; }
    }
}
