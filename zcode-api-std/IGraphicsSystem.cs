

namespace zcode_api_std
{
    public interface IGraphicsSystem
    {
        IBitmap CreateBitmap(int width, int height);
        IBitmap CreateBitmapFromFile(string filename);
        ISize CreateSize(int width, int height);
        ISizeF CreateSizeF(float width, float height);
        IRectangle CreateRectangle(int x, int y, int width, int height);
        IColorSet ColorSet { get; }
    }
}


