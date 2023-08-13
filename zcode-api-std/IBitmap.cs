

using System.IO;

namespace zcode_api_std
{
    public interface IBitmap
    {
        IGraphics CreateGraphics();
        bool BitmapIsEqualToBitmap(IBitmap bitmap);
        ISize Size { get; }
        void Save(string filename);

        MemoryStream PNGData { get; }
    }
}
