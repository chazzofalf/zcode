using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using zcode_api_std;

namespace zcode_skia
{
    internal class SkiaBitmap : IBitmap
    {
        private SkiaSharp.SKBitmap _nativeBitmap;
        private SkiaSharp.SKFont _nativeFont;

        public SkiaBitmap(SKBitmap nativeBitmap, SKFont nativeFont)
        {
            _nativeBitmap = nativeBitmap;
            _nativeFont = nativeFont;
        }
        public SkiaSharp.SKBitmap NativeBitmap => _nativeBitmap;
        public ISize Size => new SkiaSize(new SkiaSharp.SKSizeI(_nativeBitmap.Width,_nativeBitmap.Height));

        public bool BitmapIsEqualToBitmap(IBitmap bitmap)
        {
            var eq = true;
            if (bitmap is SkiaBitmap mbitmap)
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
                                //System.Console.WriteLine($"== is Killing me slowly ;-) ...(R:{current.Row}, C:{current.Column})");
                                var color_me = _nativeBitmap.GetPixel(current.Column, current.Row);
                                var color_other = _otherBitmap.GetPixel(current.Column, current.Row);
                                eq = color_me.Red == color_other.Red &&
                                color_me.Green == color_other.Green &&
                                color_me.Blue == color_other.Blue;
                            }
                            return (null);
                        });
                    //System.Console.Write($"I want to {(eq?"live":"die")}!");
                }                
            }
            return eq;
        }

        public IGraphics CreateGraphics()
        {            
            return new SkiaGraphics(() => new SkiaSharp.SKCanvas(_nativeBitmap), _nativeFont,_nativeBitmap);
        }

        public void Save(string filename)
        {
            var data = _nativeBitmap.Encode(SKEncodedImageFormat.Png, 100);
            var source = data.AsStream();
            var outf = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            System.Console.WriteLine($"Wrote to {outf.Name}");
            source.CopyTo(outf);
            source.Close();
            outf.Close();
        }
    }
}
