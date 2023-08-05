using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;

namespace zcode_win
{
    internal class WindowsGraphics : IGraphics
    {
        private System.Drawing.Graphics graphics;

        public WindowsGraphics(Graphics graphics, Font font)
        {
            this.graphics = graphics;
            this.font = font;
        }

        private System.Drawing.Font font;
        public void Clear(IColor color)
        {
            if (color is WindowColor wcolor)
            {
                this.graphics.Clear(wcolor.WindowsColor);
            }
        }

        public void DrawImage(IBitmap bitmap, IRectangle destination, IRectangle source)
        {
            if (bitmap is WindowsBitmap wbitmap &&
                destination is WindowsRectangle wdestination &&
                source is WindowsRectangle wsource)
            {
                this.graphics.DrawImage(wbitmap.WinBitmap, wdestination.WindowsRect, wsource.WindowsRect, GraphicsUnit.Pixel);
            }
            
        }

        public void DrawString(string text, IColor color)
        {
            if (color is WindowColor wcolor)
            {
                var brush = new System.Drawing.SolidBrush(wcolor.WindowsColor);
                graphics.DrawString(text, font, brush, System.Drawing.PointF.Empty);
            }
            
        }

        public ISizeF MeasureText(string text)
        {
            return new WindowsSizeF( graphics.MeasureString(text, font));
        }
    }
}
