using System.Diagnostics.CodeAnalysis;

namespace zcode;

public class ZethanaCode : IDisposable
{
    FileInfo? fontFile = null;
    private bool disposedValue;
    private static System.Drawing.Color TurquoiseColor = System.Drawing.Color.FromArgb(255,64,224,208);
    private static System.Drawing.Color BlackColor = System.Drawing.Color.FromArgb(255,0,0,0);
    private static ZethanaCode? _instance = null;
    private static ZethanaCode Instance => _instance = (_instance ?? new ZethanaCode());
    public ZethanaCode()
    {
        var tmpPath = System.IO.Path.GetTempPath();
        tmpPath = Path.Combine(tmpPath,Guid.NewGuid().ToString() + ".ttf");
        fontFile = new FileInfo(tmpPath);
        
        
        //fontFile.Create();
        var fs = new System.IO.FileStream(fontFile.FullName,FileMode.Create,FileAccess.Write);
        var inn = typeof(ZethanaCode).Assembly.GetManifestResourceStream("zcode.rsrc.zethana_font");
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
    private System.Drawing.Text.PrivateFontCollection? _privatefonts = null;
    private System.Drawing.Text.PrivateFontCollection PrivateFonts_ => (System.Drawing.Text.PrivateFontCollection)(_privatefonts ??= ((System.Func<System.Drawing.Text.PrivateFontCollection>)(() => {
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
    
    
    
    
    private System.Drawing.Font Font_ => PrivateFonts_.Families.Where(ff => ff.Name == "Zethana Monospace")
    .Select(ff => FontFromFamily(ff)).First();
    private System.Drawing.Font FontFromFamily(System.Drawing.FontFamily ff)
    {
        return new System.Drawing.Font(ff,12,System.Drawing.FontStyle.Regular);
    }
    private static System.Drawing.Font Font => Instance.Font_;
    class BitmapComparer : IEqualityComparer<System.Drawing.Bitmap>
    {
        public bool Equals(System.Drawing.Bitmap? x, System.Drawing.Bitmap? y)
        {
            if ((x != null) != (y != null))
            {
                return false;
            }
            else if (x != null && y != null)
            {
                if (ReferenceEquals(x,y))
                {
                    return true;
                }
                else
                {
                    if (x.Size.Width != y.Size.Width ||
                    x.Size.Height != y.Size.Height)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (var yci in System.Linq.Enumerable.Range(0,y.Size.Height))
                        {
                            foreach (var xci in System.Linq.Enumerable.Range(0,x.Size.Width))
                            {
                                var pixelX = x.GetPixel(xci,yci);
                                var pixelY = y.GetPixel(xci,yci);
                                if (pixelX.R != pixelY.R ||
                                pixelX.G != pixelY.G ||
                                pixelX.B != pixelY.B)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            else 
            {
                return true;
            }
            
        }

        public int GetHashCode([DisallowNull] System.Drawing.Bitmap obj)
        {
            return 0;
        }
    }
    public static string FromBitmap(System.Drawing.Bitmap b)
    {
        var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
        var bmpFont = alphas.Select(ch => {
            var isCap = char.IsUpper(ch);
            var outbi = new System.Drawing.Bitmap(22,22);
            var outb = new System.Drawing.Bitmap(24,24);
            var gb = new System.Drawing.Bitmap(1,1);
            var gg = System.Drawing.Graphics.FromImage(gb);
            var gs = gg.MeasureString($"{ch}",Font);
            gs = new System.Drawing.SizeF(gs.Width > 0 ? gs.Width : 1,gs.Height > 0 ? gs.Height : 1);
            var gss = new System.Drawing.Size((int)Math.Ceiling(gs.Width),(int)Math.Ceiling(gs.Height));
            gb = new System.Drawing.Bitmap(gss.Width,gss.Height);
            gg = System.Drawing.Graphics.FromImage(gb);
            var turquoise = new System.Drawing.SolidBrush(TurquoiseColor);
            var black = new System.Drawing.SolidBrush(BlackColor);
            gg.Clear(isCap ? TurquoiseColor : BlackColor);
            gg.DrawString($"{ch}",Font,isCap ? black : turquoise,System.Drawing.PointF.Empty);
            var outbig = System.Drawing.Graphics.FromImage(outbi);
            var outbg = System.Drawing.Graphics.FromImage(outb);
            outbig.DrawImage(gb,new System.Drawing.Rectangle(0,0,outbi.Size.Width,outbi.Size.Height),new System.Drawing.Rectangle(0,0,gb.Size.Width,gb.Size.Height),System.Drawing.GraphicsUnit.Pixel);
            outbg.Clear(isCap ? TurquoiseColor : BlackColor);
            outbg.DrawImage(outbi,new System.Drawing.Rectangle(1,1,outbi.Size.Width,outbi.Size.Height),new System.Drawing.Rectangle(0,0,outbi.Size.Width,outbi.Size.Height),System.Drawing.GraphicsUnit.Pixel);
            return (Chr:ch,bmp:outb);                       
        }).ToDictionary((item) => item.Chr,(item) => item.bmp)
        .ToDictionary((item) => item.Value,(item) => item.Key,new zcode.ZethanaCode.BitmapComparer());
        var rc = b.Size.Height/24;
        var cc = b.Size.Width/24;
        var x = Enumerable.Range(0,rc).Select(el => Enumerable.Range(0,cc).Zip(Enumerable.Repeat(el,cc),(rce,cce) => (RC:cce,CC:rce)));
        var xx = string.Join(Environment.NewLine, x.Select(xe => {
            return xe.Select( xei => {
                System.Drawing.Bitmap bseg = new System.Drawing.Bitmap(24,24);
                System.Drawing.Graphics bsegg = System.Drawing.Graphics.FromImage(bseg);
                bsegg.DrawImage(b,new System.Drawing.Rectangle(0,0,24,24),new System.Drawing.Rectangle(xei.CC*24,xei.RC*24,24,24),System.Drawing.GraphicsUnit.Pixel);
                if (!bmpFont.ContainsKey(bseg))
                {
                    return ' ';
                }
                else
                {
                    return bmpFont[bseg];
                }
            });
        }).Select(xl => {
            return (new string(xl.ToArray())).Trim();
        }).ToArray());
        return xx;
    }
    public static System.Drawing.Bitmap FromText(string s)
    {
        var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
        var bmpFont = alphas.Select(ch => {
            var isCap = char.IsUpper(ch);
            var outbi = new System.Drawing.Bitmap(22,22);
            var outb = new System.Drawing.Bitmap(24,24);
            var gb = new System.Drawing.Bitmap(1,1);
            var gg = System.Drawing.Graphics.FromImage(gb);
            var gs = gg.MeasureString($"{ch}",Font);
            gs = new System.Drawing.SizeF(gs.Width > 0 ? gs.Width : 1,gs.Height > 0 ? gs.Height : 1);
            var gss = new System.Drawing.Size((int)Math.Ceiling(gs.Width),(int)Math.Ceiling(gs.Height));
            gb = new System.Drawing.Bitmap(gss.Width,gss.Height);
            gg = System.Drawing.Graphics.FromImage(gb);
            var turquoise = new System.Drawing.SolidBrush(TurquoiseColor);
            var black = new System.Drawing.SolidBrush(BlackColor);
            gg.Clear(isCap ? TurquoiseColor : BlackColor);
            gg.DrawString($"{ch}",Font,isCap ? black : turquoise,System.Drawing.PointF.Empty);
            var outbig = System.Drawing.Graphics.FromImage(outbi);
            var outbg = System.Drawing.Graphics.FromImage(outb);
            outbig.DrawImage(gb,new System.Drawing.Rectangle(0,0,outbi.Size.Width,outbi.Size.Height),new System.Drawing.Rectangle(0,0,gb.Size.Width,gb.Size.Height),System.Drawing.GraphicsUnit.Pixel);
            outbg.Clear(isCap ? TurquoiseColor : BlackColor);
            outbg.DrawImage(outbi,new System.Drawing.Rectangle(1,1,outbi.Size.Width,outbi.Size.Height),new System.Drawing.Rectangle(0,0,outbi.Size.Width,outbi.Size.Height),System.Drawing.GraphicsUnit.Pixel);
            return (Chr:ch,bmp:outb);                       
        }).ToDictionary((item) => item.Chr,(item) => item.bmp);
        var length = s.Length;
        var line_length = ((int)(Math.Sqrt(length)))+1;
        line_length = line_length > 80 ? line_length : 80;
        var lines = s.Split(Environment.NewLine);
        var rlines = System.Linq.Enumerable.Empty<string>();
        foreach (var l in lines)
        {
            var words = l.Split(' ');
            var cline = "";
            var lie = true;
            foreach (var w in words)
            {
                lie = false;
                if (cline.Length == 0)
                {
                    cline = w;
                }
                else
                {
                    cline = $"{cline} {w}";
                }
                
                while (cline.Length > line_length)
                {
                    if (cline.Contains(" "))
                    {
                        var idx = cline.Length-1;
                        while (idx >= line_length || cline[idx] != ' ')
                        {
                            idx--;
                        }
                        rlines = rlines.Append(cline[..idx]);
                        cline = cline[idx..];
                    }
                    else
                    {                        
                        rlines = rlines.Append(cline[..line_length]);
                        cline = cline[line_length..];                        
                    }
                }                
                           
            }
            if (cline.Length > 0)
            {
                rlines = rlines.Append(cline);
            }
            if (lie)
            {
                rlines = rlines.Append("");
            }
        }
        var ce = rlines.Select((l) => l.Select(lc => lc));
        var lc = ce.Count();
        var cc = ce.Select(l => l.Count())
        .OrderByDescending(c => c)
        .FirstOrDefault();
        var bm = new System.Drawing.Bitmap(24*cc,24*lc);        
        var bmg = System.Drawing.Graphics.FromImage(bm);
        bmg.Clear(BlackColor);
        var lci =0;
        var cci =0;
        ce.Aggregate((object)null,(prev,cur) => {
            cci = 0;
            cur.Aggregate((object)null,(previ,curi) => {
                var curir = curi;
                if (!bmpFont.ContainsKey(curi))
                {
                    curir = ' ';
                }
                bmg.DrawImage(bmpFont[curir],new System.Drawing.Rectangle(cci*24,lci*24,24,24),new System.Drawing.Rectangle(0,0,24,24),System.Drawing.GraphicsUnit.Pixel);
                cci++;
                return null;
            });
            lci++;
            return null;
        });
        
        
        
        return bm;
        

        
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                fontFile.Delete();                
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ZethanaCode()
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
}

