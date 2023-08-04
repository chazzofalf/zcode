namespace zcode_api;
public interface IGraphics
{
    public ISizeF MeasureText(string text);
    public void DrawString(string text,IColor color);
    public void Clear(IColor color);    
    public void DrawImage(IBitmap bitmap,IRectangle destination,IRectangle source);
}