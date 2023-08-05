using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowsRectangle : IRectangle
    {
        public System.Drawing.Rectangle WindowsRect{ get; }

        public int X => WindowsRect.X;

        public int Y => WindowsRect.Y;

        public int Width => WindowsRect.Width;

        public int Height => WindowsRect.Height;

        public WindowsRectangle(Rectangle windowsRectangle)
        {
            WindowsRect = windowsRectangle;
        }
    }
}
