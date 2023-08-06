using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;
namespace zcode_win
{
    public class WindowsGraphicsSystem : IGraphicsSystem,IDisposable
    {
        private bool disposedValue;
        FileInfo fontFile = null;
        public WindowsGraphicsSystem() 
        { 
            disposedValue = false;
            var tmpPath = System.IO.Path.GetTempPath();
            tmpPath = Path.Combine(tmpPath, Guid.NewGuid().ToString() + ".ttf");
            fontFile = new FileInfo(tmpPath);


            //fontFile.Create();
            var fs = new System.IO.FileStream(fontFile.FullName, FileMode.Create, FileAccess.Write);
            var inn = typeof(zcode_rsrcs.Resources).Assembly.GetManifestResourceStream(zcode_rsrcs.Resources.FontResourceName);
            if (inn != null)
            {
                inn.CopyTo(fs);
                inn.Close();
                fs.Close();
            }
            else
            {
                throw new Exception("Could not open the embedded font!");
            }
        }

        private System.Drawing.Text.PrivateFontCollection _privatefonts = null;
        private System.Drawing.Text.PrivateFontCollection PrivateFonts_ => (System.Drawing.Text.PrivateFontCollection)(_privatefonts = _privatefonts != null ? _privatefonts :  ((System.Func<System.Drawing.Text.PrivateFontCollection>)(() => {
            if (fontFile != null)
            {
                var pf = new System.Drawing.Text.PrivateFontCollection();
                pf.AddFontFile(fontFile.FullName);
                return pf;
            }
            else
            {
                throw new Exception("Could not create font file.");
            }

        }))());
        private System.Drawing.Font Font => PrivateFonts_.Families.Where(ff => ff.Name == "Zethana Monospace")
    .Select(ff => FontFromFamily(ff)).First();
        private System.Drawing.Font FontFromFamily(System.Drawing.FontFamily ff)
        {
            return new System.Drawing.Font(ff, 12, System.Drawing.FontStyle.Regular);
        }
        private IColorSet _colorset;
        public IColorSet ColorSet => _colorset = _colorset != null ? _colorset : ((Func<IColorSet>)(() => {
            return new WindowsColorSet();
        }))();

        public IBitmap CreateBitmap(int width, int height)
        {
            var b = new System.Drawing.Bitmap(width, height);
            return new WindowsBitmap(b,Font);
        }

        public IRectangle CreateRectangle(int x, int y, int width, int height)
        {
            var r = new System.Drawing.Rectangle(x, y, width, height);
            return new WindowsRectangle(r);
        }

        public ISize CreateSize(int width, int height)
        {
            var s = new System.Drawing.Size(width,height);
            return new WindowsSize(s);
        }

        public ISizeF CreateSizeF(float width, float height)
        {
            var sf = new System.Drawing.SizeF(width,height);
            return new WindowsSizeF(sf);
        }

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
        // ~WindowsGraphicsSystem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IBitmap CreateBitmapFromFile(string filename)
        {
            var b = System.Drawing.Bitmap.FromFile(filename);
            var bb = new System.Drawing.Bitmap(b.Width,b.Height);
            var bbg = System.Drawing.Graphics.FromImage(bb);
            bbg.DrawImage(b,0,0);
            return new WindowsBitmap (bb,Font);
        }
    }
}
