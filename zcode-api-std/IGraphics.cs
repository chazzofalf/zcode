

namespace zcode_api_std
{
    public interface IGraphics
    {
        ISizeF MeasureText(string text);
        void DrawString(string text, IColor color);
        void Clear(IColor color);
        void DrawImage(IBitmap bitmap, IRectangle destination, IRectangle source);
    }
}
