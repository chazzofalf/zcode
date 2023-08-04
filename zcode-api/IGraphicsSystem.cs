namespace zcode_api;

public interface IGraphicsSystem
{
    public IBitmap CreateBitmap(int width,int height);
    public ISize CreateSize(int width,int height);
    public ISizeF CreateSizeF(float width,float height);
    public IRectangle CreateRectangle(int x,int y,int width,int height);
    public IColorSet ColorSet {get;}
}
