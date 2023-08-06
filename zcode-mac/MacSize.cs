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
    internal class MacSize : ISize
    {
        private SkiaSharp.SKSizeI _skSize;

        public MacSize(SKSizeI skSize)
        {
            _skSize = skSize;
        }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();
    }
}
