using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zcode_api_std;
using zcode_common_std;
namespace zcode_skia
{
    public class SkiaGraphicsSystem : IGraphicsSystem, IDisposable
    {
        public SkiaGraphicsSystem()
        {
            //SkiaSharp.SKGraphics.Init();
        }
        private bool disposedValue;

        private Util.LazyVariable<IColorSet> _colorSet = Util.LazyVariable<IColorSet>.Create((Func<IColorSet>)(() =>
        {
            return (IColorSet)(new SkiaColorSet());
        }));
        public IColorSet ColorSet => _colorSet.Get;
            

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MacGraphicsSystem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        private Util.LazyVariable<SkiaSharp.SKFont> _zisFont = Util.LazyVariable<SkiaSharp.SKFont>.Create(() =>
        {
            var typeface = SkiaSharp.SKTypeface.FromStream(typeof(zcode_rsrcs.Resources).Assembly.GetManifestResourceStream(zcode_rsrcs.Resources.FontResourceName));
            //SkiaSharp.SKTypeface.Default;
            var font = new SkiaSharp.SKFont(typeface, 1024);
            
            return font;

        });
        private SkiaSharp.SKFont Font => _zisFont.Get;
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        public IBitmap CreateBitmap(int width, int height)
        {
            var image = new SkiaSharp.SKBitmap(width, height);
            return new SkiaBitmap(image,Font);
        }

        public IBitmap CreateBitmapFromFile(string filename)
        {
            var fio = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read);
            var bio = new System.IO.MemoryStream();
            fio.CopyTo(bio);
            fio.Close();
            bio.Seek(0, SeekOrigin.Begin);
            



            var bmp = SkiaSharp.SKBitmap.Decode(bio);
            if (bmp != null)
            {
                var gbmp = new SkiaSharp.SKBitmap(bmp.Width, bmp.Height);                
                var can = new SkiaSharp.SKCanvas(gbmp);
                can.DrawBitmap(bmp, SkiaSharp.SKPoint.Empty);
                return new SkiaBitmap(gbmp, Font);
            }
            return null;

            
        }

        public ISize CreateSize(int width, int height)
        {
            var sksize = new SkiaSharp.SKSizeI(width, height);
            return new SkiaSize(sksize);
        }

        public ISizeF CreateSizeF(float width, float height)
        {
            var sksizef = new SkiaSharp.SKSize(width, height);
            return new SkiaSizeF(sksizef);
        }

        public IRectangle CreateRectangle(int x, int y, int width, int height)
        {
            var skRect = new SkiaSharp.SKRectI(x, y, x + width, y + height);
            return new SkiaRectangle(skRect);
        }
    }
}
