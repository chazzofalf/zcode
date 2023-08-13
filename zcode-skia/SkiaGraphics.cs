using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using zcode_api_std;
using static System.Net.Mime.MediaTypeNames;

namespace zcode_skia
{
    internal class SkiaGraphics : IGraphics
    {
        private Func<SKCanvas> canvasCreator;
        private SKFont nativeFont;
        private SKBitmap nativeBitmap;

        public SkiaGraphics(Func<SKCanvas> canvasCreator, SKFont nativeFont,SKBitmap nativeBitmap)
        {
            this.canvasCreator = canvasCreator;
            this.nativeFont = nativeFont;
            this.nativeBitmap = nativeBitmap;
        }

        public void Clear(IColor color)
        {
            if (color is SkiaColor mColor)
            {
                using (var canvas = canvasCreator())
                {
                    canvas.Clear(mColor.NativeColor);
                }
                
                
            }
        }
        private int imgseq = 0;
        private void SaveBitmap(SKBitmap bmp)
        {
            var data = bmp.Encode(SKEncodedImageFormat.Png, 100);
            var datas = data.AsStream();
            var dtnow = DateTime.Now;
            var fio = new System.IO.FileStream($"rbs_zethana_bmp_save_{dtnow.Year}_{dtnow.Month}_{dtnow.Day}_{dtnow.Hour}_{dtnow.Minute}_{dtnow.Second}_{dtnow.Millisecond}_{imgseq++}.png",System.IO.FileMode.Create,System.IO.FileAccess.Write);
            datas.CopyTo(fio);
            fio.Close();
        }
        public void DrawImage(IBitmap bitmap, IRectangle destination, IRectangle source)
        {
            if (destination is SkiaRectangle mDestination &&
                source is SkiaRectangle mSource &&
                bitmap is SkiaBitmap mImage)
            {
                using (var canvas = canvasCreator())
                {
                    if (mImage.NativeBitmap.Width < 100 || mImage.NativeBitmap.Height < 100)
                    {
                        int i = 5;
                        i++;
                    }
                    //SaveBitmap(mImage.NativeBitmap);
                    canvas.DrawBitmap(mImage.NativeBitmap,  mSource.NativeRect, mDestination.NativeRect);
                    
                }
                
            }
            
        }
        private void DrawStringDebug(string text, IColor color)
        {
            SKBitmap tb = PregenerateGlyph(text, color);

            var data = tb.Encode(SKEncodedImageFormat.Png, 100);
            var pfn = $"pfn_zethana_{(char.IsUpper(text[0]) ? "_" : "")}{text[0]}.png";
            var data_stream = data.AsStream();
            var fio = new System.IO.FileStream(pfn, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            data_stream.CopyTo(fio);
            fio.Flush();
            fio.Close();
        }

        private SKBitmap PregenerateGlyph(string text, IColor color)
        {
            var brush = new SKPaint(this.nativeFont);
            var rect = SKRect.Empty;
            brush.MeasureText(text, ref rect);

            //var mt = this.MeasureText(text);
            var fmt = new SkiaSize(new SKSizeI((int)(rect.Width == 0 ? 1 : Math.Ceiling(rect.Width)), (int)(rect.Height == 0 ? 1 : Math.Ceiling(rect.Height))));
            var tb = new SKBitmap(fmt.Width, fmt.Height);


            using (var tg = new SKCanvas(tb))
            {

                brush.Color = (SKColor)(color as SkiaColor)?.NativeColor;
                if (char.IsUpper(text[0]))
                {
                    var tur = new SkiaColor(new SkiaSharp.SKColor(64, 224, 208));
                    var turc = tur.NativeColor;

                    tg.Clear(turc);
                }
                else
                {
                    var tur = new SkiaColor(new SkiaSharp.SKColor(0, 0, 0));
                    var turc = tur.NativeColor;
                    tg.Clear(turc);
                }

                tg.DrawText(text, -rect.Left, -rect.Top, brush);
            }

            return tb;
        }

        public void DrawString(string text, IColor color)
        {
            if (color is SkiaColor mColor)
            {
                var pg = PregenerateGlyph(text, color);
                
                using (var canvas = canvasCreator())
                {
                    canvas.DrawBitmap(pg,new SKRect(0, 0,pg.Width,pg.Height),new SKRect(0,0,nativeBitmap.Width,nativeBitmap.Height));

                    
                    
                    
                    
                }
                //DrawStringDebug(text, color);


            }
            
        }

        public ISizeF MeasureText(string text)
        {
            var pg = PregenerateGlyph(text, new SkiaColor(new SKColor(255, 255, 255)));
            var ssz = new SkiaSizeF(new SKSize(pg.Width, pg.Height));
            
            return ssz;
        }
    }
}
