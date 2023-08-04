namespace zcode_api;
public interface IBitmap
{
    public IGraphics CreateGraphics();
    public bool BitmapIsEqualToBitmap(IBitmap bitmap);
    public ISize Size {get;}
}