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

        public int X => throw new NotImplementedException();

        public int Y => throw new NotImplementedException();

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();
    }
}
