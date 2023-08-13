using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_skia
{
    internal class SkiaColor : IColor
    {
        private SkiaSharp.SKColor _skColor;
        public SkiaSharp.SKColor NativeColor => _skColor;
        public SkiaColor(SkiaSharp.SKColor skColor)
        {
            _skColor = skColor;
        }
    }
}
