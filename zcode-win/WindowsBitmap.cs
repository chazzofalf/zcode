using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;
using zcode_common_std;
namespace zcode_win
{
    internal class WindowsBitmap : IBitmap
    {
        private System.Drawing.Bitmap _bitmap;
        private System.Drawing.Font _font;
        public System.Drawing.Bitmap WinBitmap { get { return _bitmap; } }
        public WindowsBitmap(System.Drawing.Bitmap bitmap, System.Drawing.Font font)
        {
            _bitmap = bitmap;
            _font = font;
            _size = Util.LazyVariable<ISize>.Create(() =>
            {
                return new WindowsSize(_bitmap.Size);
            });
        }
        private Util.LazyVariable<ISize> _size;
        public ISize Size => _size.Get;

        public bool BitmapIsEqualToBitmap(IBitmap bitmap)
        {
            if (bitmap is WindowsBitmap wbitmap)
            {
                if (Size.Width != Size.Width ||
                    Size.Height != Size.Height)
                {
                    return false;
                }
                else
                {
                    foreach (var yci in System.Linq.Enumerable.Range(0, Size.Height))
                    {
                        foreach (var xci in System.Linq.Enumerable.Range(0, Size.Width))
                        {
                            var pixelX = _bitmap.GetPixel(xci, yci);
                            var pixelY = wbitmap._bitmap.GetPixel(xci, yci);
                            if (pixelX.R != pixelY.R ||
                            pixelX.G != pixelY.G ||
                            pixelX.B != pixelY.B)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public IGraphics CreateGraphics()
        {
            var g = System.Drawing.Graphics.FromImage(WinBitmap);
            return new WindowsGraphics(g, _font);
        }

        public void Save(string filename)
        {
            _bitmap.Save(filename);
        }
    }
}
