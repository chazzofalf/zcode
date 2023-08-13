using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using zcode_api_std;

namespace zcode_skia
{
    internal class SkiaSize : ISize
    {
        private SkiaSharp.SKSizeI _skSize;

        public SkiaSize(SKSizeI skSize)
        {
            _skSize = skSize;
        }

        public int Width => _skSize.Width;

        public int Height => _skSize.Height;
    }
}
