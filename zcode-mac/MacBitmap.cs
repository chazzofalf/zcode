using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using zcode_api_std;

namespace zcode_mac
{
    internal class MacBitmap : IBitmap
    {
        private SkiaSharp.SKBitmap _nativeBitmap;
        private SkiaSharp.SKFont _nativeFont;

        public MacBitmap(SKBitmap nativeBitmap, SKFont nativeFont)
        {
            _nativeBitmap = nativeBitmap;
            _nativeFont = nativeFont;
        }

        public ISize Size => new MacSize(new SkiaSharp.SKSizeI(_nativeBitmap.Width,_nativeBitmap.Height));

        public bool BitmapIsEqualToBitmap(IBitmap bitmap)
        {
            var eq = true;
            if (bitmap is MacBitmap mbitmap)
            {
                var _otherBitmap = mbitmap._nativeBitmap;
                eq = _nativeBitmap.Height == _otherBitmap.Height &&
                    _nativeBitmap.Width == _otherBitmap.Width;
                if (eq)
                {
                    Enumerable.Range(0, _nativeBitmap.Height)
                        .Select(r => Enumerable.Range(0, _nativeBitmap.Width).
                        Select(c => (Row: r, Column: c)))
                        .SelectMany(rc => rc)
                        .Aggregate((object)null, (prev, current) =>
                        {
                            if (eq)
                            {
                                var color_me = _nativeBitmap.GetPixel(current.Column, current.Row);
                                var color_other = _otherBitmap.GetPixel(current.Column, current.Row);
                                eq = color_me.Red == color_other.Red &&
                                color_me.Green == color_other.Green &&
                                color_me.Blue == color_other.Blue;
                            }
                            return (null);
                        });
                }                
            }
            return eq;
        }

        public IGraphics CreateGraphics()
        {
            var canvas = new SkiaSharp.SKCanvas(_nativeBitmap);
            return new MacGraphics(canvas, _nativeFont);
        }

        public void Save(string filename)
        {
            var data = _nativeBitmap.Encode(SKEncodedImageFormat.Png, 100);
            var source = data.AsStream();
            var outf = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            source.CopyTo(outf);
            source.Close();
            outf.Close();
        }
    }
}
