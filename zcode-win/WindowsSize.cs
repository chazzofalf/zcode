using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowsSize : ISize
    {
        private System.Drawing.Size _size;

        public WindowsSize(Size size)
        {
            _size = size;
        }

        public int Width => _size.Width;

        public int Height => _size.Height;
    }
}
