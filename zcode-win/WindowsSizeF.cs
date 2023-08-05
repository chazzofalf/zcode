using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowsSizeF : ISizeF
    {
        private System.Drawing.SizeF size;

        public WindowsSizeF(SizeF size)
        {
            this.size = size;
        }

        public float Width => size.Width;

        public float Height => size.Height;
    }
}
