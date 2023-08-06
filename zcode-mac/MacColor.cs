using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_mac
{
    internal class MacColor : IColor
    {
        private SkiaSharp.SKColor _skColor;
        public SkiaSharp.SKColor NativeColor => _skColor;
        public MacColor(SkiaSharp.SKColor skColor)
        {
            _skColor = skColor;
        }
    }
}
