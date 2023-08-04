using System.Diagnostics.CodeAnalysis;
using zcode_api;
namespace zcode_base;

public class ZethanaCode : IDisposable
{
    private IGraphicsSystem graphicsSystem;
    
    
    private bool disposedValue;
    
    
    
    public ZethanaCode(IGraphicsSystem graphics)
    {
        
        this.graphicsSystem = graphics;
        
    }
        
    class BitmapComparer : IEqualityComparer<zcode_api.IBitmap>
    {
        public bool Equals(zcode_api.IBitmap? x, zcode_api.IBitmap? y)
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
                    return x.BitmapIsEqualToBitmap(y);
                }
            }
            else 
            {
                return true;
            }
            
        }

        public int GetHashCode([DisallowNull] zcode_api.IBitmap obj)
        {
            return 0;
        }
    }
    public  string FromBitmap(zcode_api.IBitmap b)
    {
        var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
        var bmpFont = alphas.Select(ch => {
            var isCap = char.IsUpper(ch);
            var outbi =  graphicsSystem.CreateBitmap(22,22); 
            var outb =  graphicsSystem.CreateBitmap(24,24);
            var gb = graphicsSystem.CreateBitmap(1,1); 
            var gg = gb.CreateGraphics();
            var gs = gg.MeasureText($"{ch}");
            gs = graphicsSystem.CreateSizeF(gs.Width > 0 ? gs.Width : 1,gs.Height > 0 ? gs.Height : 1);
            var gss = graphicsSystem.CreateSize((int)Math.Ceiling(gs.Width),(int)Math.Ceiling(gs.Height));
            gb = graphicsSystem.CreateBitmap(gss.Width,gss.Height);
            gg = gb.CreateGraphics();
            gg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
            gg.DrawString($"{ch}",isCap ? graphicsSystem.ColorSet.Black : graphicsSystem.ColorSet.Turquoise);
            var outbig = outbi.CreateGraphics();
            var outbg = outb.CreateGraphics();
            outbig.DrawImage(gb,graphicsSystem.CreateRectangle(0,0,outbi.Size.Width,outbi.Size.Height),graphicsSystem.CreateRectangle(0,0,gb.Size.Width,gb.Size.Height));
            outbg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
            outbg.DrawImage(outbi,graphicsSystem.CreateRectangle(1,1,outbi.Size.Width,outbi.Size.Height),graphicsSystem.CreateRectangle(0,0,outbi.Size.Width,outbi.Size.Height));
            return (Chr:ch,bmp:outb);                       
        }).ToDictionary((item) => item.Chr,(item) => item.bmp)
        .ToDictionary((item) => item.Value,(item) => item.Key,new zcode_base.ZethanaCode.BitmapComparer());
        var rc = b.Size.Height/24;
        var cc = b.Size.Width/24;
        var x = Enumerable.Range(0,rc).Select(el => Enumerable.Range(0,cc).Zip(Enumerable.Repeat(el,cc),(rce,cce) => (RC:cce,CC:rce)));
        var xx = string.Join(Environment.NewLine, x.Select(xe => {
            return xe.Select( xei => {
                var bseg = graphicsSystem.CreateBitmap(24,24);
                var bsegg = bseg.CreateGraphics();
                
                bsegg.DrawImage(b,graphicsSystem.CreateRectangle(0,0,24,24),graphicsSystem.CreateRectangle(xei.CC*24,xei.RC*24,24,24));
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
    public  zcode_api.IBitmap FromText(string s)
    {
        var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
        var bmpFont = alphas.Select(ch => {
            var isCap = char.IsUpper(ch);
            var outbi = graphicsSystem.CreateBitmap(22,22);
            var outb = graphicsSystem.CreateBitmap(24,24);
            var gb = graphicsSystem.CreateBitmap(1,1);
            var gg = gb.CreateGraphics();
            var gs = gg.MeasureText($"{ch}");
            gs = graphicsSystem.CreateSizeF(gs.Width > 0 ? gs.Width : 1,gs.Height > 0 ? gs.Height : 1);
            var gss = graphicsSystem.CreateSize((int)Math.Ceiling(gs.Width),(int)Math.Ceiling(gs.Height));
            gb = graphicsSystem.CreateBitmap(gss.Width,gss.Height);
            gg = gb.CreateGraphics();            
            gg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
            gg.DrawString($"{ch}",isCap ? graphicsSystem.ColorSet.Black : graphicsSystem.ColorSet.Turquoise);
            var outbig = outbi.CreateGraphics();
            var outbg = outb.CreateGraphics();
            outbig.DrawImage(gb,graphicsSystem.CreateRectangle(0,0,outbi.Size.Width,outbi.Size.Height),graphicsSystem.CreateRectangle(0,0,gb.Size.Width,gb.Size.Height));
            outbg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
            outbg.DrawImage(outbi,graphicsSystem.CreateRectangle(1,1,outbi.Size.Width,outbi.Size.Height),graphicsSystem.CreateRectangle(0,0,outbi.Size.Width,outbi.Size.Height));
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
        var bm = graphicsSystem.CreateBitmap(24*cc,24*lc);        
        var bmg = bm.CreateGraphics();
        bmg.Clear(graphicsSystem.ColorSet.Black);
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
                bmg.DrawImage(bmpFont[curir],graphicsSystem.CreateRectangle(cci*24,lci*24,24,24),graphicsSystem.CreateRectangle(0,0,24,24));
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
                //fontFile.Delete();                
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

