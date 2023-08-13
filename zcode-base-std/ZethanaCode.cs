using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using zcode_api_std;
using System.Threading.Tasks;
using System.IO;

namespace zcode_base
{
    public class ZethanaCode : IDisposable
    {
        private IGraphicsSystem graphicsSystem;
        private ZethanaFontCache fc;

        private bool disposedValue;

        public static async Task<ZethanaCode>InitAsync(IGraphicsSystem graphicsSystem)
        {
            await Task.Yield();
            return new ZethanaCode(graphicsSystem);
        }

        public ZethanaCode(IGraphicsSystem graphics)
        {

            this.graphicsSystem = graphics;
            this.fc = new ZethanaFontCache(graphics);
        }

        public async Task<string>FromBitmapAsync(zcode_api_std.IBitmap b)
        {
            await Task.Yield();
            return FromBitmap(b);
        }
        public string FromBitmap(string filename)
        {
            return FromBitmap(graphicsSystem.CreateBitmapFromFile(filename));
        }
        public string FromBitmap(zcode_api_std.IBitmap b)
        {
            var bmpFont = fc.ReverseFontCacheDictionary;
            var rc = b.Size.Height / 24;
            var cc = b.Size.Width / 24;
            var x = Enumerable.Range(0, rc).Select(el => Enumerable.Range(0, cc).Zip(Enumerable.Repeat(el, cc), (rce, cce) => (RC: cce, CC: rce)));
            var xx = string.Join(Environment.NewLine, Extractline(b, bmpFont, x)

                );

            /*(.Select(xl => {

            return (new string(xl.ToArray())).Trim();
        }).ToArray());*/
            return xx;
        }

        private IEnumerable<string> Extractline(IBitmap b, Dictionary<IBitmap, char> bmpFont, IEnumerable<IEnumerable<(int RC, int CC)>> x)
        {
            var outxt=  x.Select(xe =>
            {
                var asyncout = xe.Select(xei =>
                {
                    var bseg = graphicsSystem.CreateBitmap(24, 24);
                    var bsegg = bseg.CreateGraphics();

                    bsegg.DrawImage(b, graphicsSystem.CreateRectangle(0, 0, 24, 24), graphicsSystem.CreateRectangle(xei.CC * 24, xei.RC * 24, 24, 24));
                    if (!bmpFont.ContainsKey(bseg))
                    {
                        return Task.Run(async () =>
                        {
                            await Task.Yield();
                            return ' ';
                        });

                    }
                    else
                    {
                        return Task.Run(async () =>
                        {
                            await Task.Yield();
                            return bmpFont[bseg];
                        });

                    }
                });
                var txtt = Task.Run(async () =>
                {
                    await Task.Yield();
                    return new string(await Task.WhenAll(asyncout));
                });
                return txtt;



            });
            var outxtc = Task.WhenAll(outxt);
            return outxtc.Result;
        }
        
        public async Task<zcode_api_std.IBitmap> FromTextAsync(string s)
        {
            await Task.Yield();
            return FromText(s);
        }
        public zcode_api_std.IBitmap FromText(string s)
        {
            var bmpFont = fc.FontCacheDictionary;
            
            //bmpFont.Aggregate((object)null, (ignore, current) =>
            //{
            //    current.Value.Save($"mac_zethana_{(char.IsUpper(current.Key) ? "_" : "")}{current.Key}.png");
            //    return null;
            //});
            var length = s.Length;
            var line_length = ((int)(Math.Sqrt(length))) + 1;
            line_length = line_length > 80 ? line_length : 80;
            var lines = s.Split('\n');
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
                            var idx = cline.Length - 1;
                            while (idx >= line_length || cline[idx] != ' ')
                            {
                                idx--;
                            }
                            rlines = rlines.Append(cline.Substring(0,idx) /*cline[..idx]*/);
                            cline = cline.Substring(idx) /*cline[idx..]*/;
                        }
                        else
                        {
                            rlines = rlines.Append(cline.Substring(0,line_length /*cline[..line_length]*/));
                            cline = cline.Substring(line_length) /*cline[line_length..]*/;
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
            var ce = rlines.Select((l) => l.Select(lc_ => lc_));
            var lc = ce.Count();
            var cc = ce.Select(l => l.Count())
            .OrderByDescending(c => c)
            .FirstOrDefault();
            var bm = graphicsSystem.CreateBitmap(24 * cc, 24 * lc);
            var bmg = bm.CreateGraphics();
            bmg.Clear(graphicsSystem.ColorSet.Black);
            var lci = 0;
            var cci = 0;
            ce.Aggregate((object)null, (prev, cur) => {
                cci = 0;
                cur.Aggregate((object)null, (previ, curi) => {
                    var curir = curi;
                    if (!bmpFont.ContainsKey(curi))
                    {
                        curir = ' ';
                    }
                    bmg.DrawImage(bmpFont[curir], graphicsSystem.CreateRectangle(cci * 24, lci * 24, 24, 24), graphicsSystem.CreateRectangle(0, 0, 24, 24));
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
}



