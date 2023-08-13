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
    internal class SkiaSizeF : ISizeF
    {
        private SKSize sksizef;

        public SkiaSizeF(SKSize sksizef)
        {
            this.sksizef = sksizef;
        }

        public float Width => sksizef.Width;

        public float Height => sksizef.Height;
    }
}
