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

namespace zcode_mac
{
    internal class MacGraphics : IGraphics
    {
        private SKCanvas canvas;
        private SKFont nativeFont;

        public MacGraphics(SKCanvas canvas, SKFont nativeFont)
        {
            this.canvas = canvas;
            this.nativeFont = nativeFont;
        }

        public void Clear(IColor color)
        {
            if (color is MacColor mColor)
            {
                this.canvas.Clear(mColor.NativeColor);
            }
        }

        public void DrawImage(IBitmap bitmap, IRectangle destination, IRectangle source)
        {
            if (destination is MacRectangle mDestination &&
                source is MacRectangle mSource &&
                bitmap is MacBitmap mImage)
            {
                this.canvas.DrawBitmap(mImage.NativeBitmap, mDestination.NativeRect, mSource.NativeRect);
            }
            
        }

        public void DrawString(string text, IColor color)
        {
            if (color is MacColor mColor)
            {
                var brush = new SKPaint();
                brush.Color = mColor.NativeColor;
                this.canvas.DrawText(text, SKPoint.Empty, brush);
            }            
        }

        public ISizeF MeasureText(string text)
        {
            var brush = new SKPaint(this.nativeFont);
            var skrect = SKRect.Empty;
            var szn = brush.MeasureText(text,ref skrect);
            var sz = new MacSizeF(skrect.Size);
            return sz;
        }
    }
}
