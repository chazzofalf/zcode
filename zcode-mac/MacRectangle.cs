using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using zcode_api_std;

namespace zcode_mac
{
    internal class MacRectangle : IRectangle
    {
        private SKRectI skRect;

        public MacRectangle(SKRectI skRect)
        {
            this.skRect = skRect;
        }
        public SKRectI NativeRect => skRect;

        public int X => skRect.Left;

        public int Y => skRect.Top;

        public int Width => skRect.Right-skRect.Left;

        public int Height => skRect.Bottom-skRect.Top;
    }
}
