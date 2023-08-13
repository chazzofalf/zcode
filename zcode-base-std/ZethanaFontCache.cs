using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_api_std;
using zcode_common_std;
namespace zcode_base
{
    internal class ZethanaFontCache
    {
        private Dictionary<IBitmap, char> _reverseDictionary;
        private Dictionary<char,IBitmap> _dictionary;

        public Dictionary<char, IBitmap> FontCacheDictionary => _dictionary;
        public Dictionary<IBitmap, char> ReverseFontCacheDictionary => _reverseDictionary;
        
        
        
        
        public ZethanaFontCache(IGraphicsSystem graphicsSystemArg)
        {
            var graphicsSystem = graphicsSystemArg;            
            var task1 = Task.Run(async () =>
            {
                await Task.Yield();
                var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
                _dictionary = alphas.Select(ch => {
                                        
                    var isCap = char.IsUpper(ch);
                    var outbi = graphicsSystem.CreateBitmap(22, 22);
                    var outb = graphicsSystem.CreateBitmap(24, 24);
                    var gb = graphicsSystem.CreateBitmap(1, 1);
                    var gg = gb.CreateGraphics();
                    var gs = gg.MeasureText($"{ch}");
                    gs = graphicsSystem.CreateSizeF(gs.Width > 0 ? gs.Width : 1, gs.Height > 0 ? gs.Height : 1);
                    var gss = graphicsSystem.CreateSize((int)Math.Ceiling(gs.Width), (int)Math.Ceiling(gs.Height));
                    gb = graphicsSystem.CreateBitmap(gss.Width, gss.Height);
                    gg = gb.CreateGraphics();
                    gg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
                    gg.DrawString($"{ch}", isCap ? graphicsSystem.ColorSet.Black : graphicsSystem.ColorSet.Turquoise);
                    var outbig = outbi.CreateGraphics();
                    var outbg = outb.CreateGraphics();
                    outbig.DrawImage(gb, graphicsSystem.CreateRectangle(0, 0, outbi.Size.Width, outbi.Size.Height), graphicsSystem.CreateRectangle(0, 0, gb.Size.Width, gb.Size.Height));
                    outbg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
                    outbg.DrawImage(outbi, graphicsSystem.CreateRectangle(1, 1, outbi.Size.Width, outbi.Size.Height), graphicsSystem.CreateRectangle(0, 0, outbi.Size.Width, outbi.Size.Height));
                    return (Chr: ch, bmp: outb);
                }).ToDictionary((item) => item.Chr, (item) => item.bmp);
            });
            var task2 = Task.Run(async () =>
            {
                await Task.Yield();
                var alphas = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
                _reverseDictionary = alphas.Select(ch => {
                    var isCap = char.IsUpper(ch);
                    var outbi = graphicsSystem.CreateBitmap(22, 22);
                    var outb = graphicsSystem.CreateBitmap(24, 24);
                    var gb = graphicsSystem.CreateBitmap(1, 1);
                    var gg = gb.CreateGraphics();
                    var gs = gg.MeasureText($"{ch}");
                    gs = graphicsSystem.CreateSizeF(gs.Width > 0 ? gs.Width : 1, gs.Height > 0 ? gs.Height : 1);
                    var gss = graphicsSystem.CreateSize((int)Math.Ceiling(gs.Width), (int)Math.Ceiling(gs.Height));
                    gb = graphicsSystem.CreateBitmap(gss.Width, gss.Height);
                    gg = gb.CreateGraphics();
                    gg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
                    gg.DrawString($"{ch}", isCap ? graphicsSystem.ColorSet.Black : graphicsSystem.ColorSet.Turquoise);
                    var outbig = outbi.CreateGraphics();
                    var outbg = outb.CreateGraphics();
                    outbig.DrawImage(gb, graphicsSystem.CreateRectangle(0, 0, outbi.Size.Width, outbi.Size.Height), graphicsSystem.CreateRectangle(0, 0, gb.Size.Width, gb.Size.Height));
                    outbg.Clear(isCap ? graphicsSystem.ColorSet.Turquoise : graphicsSystem.ColorSet.Black);
                    outbg.DrawImage(outbi, graphicsSystem.CreateRectangle(1, 1, outbi.Size.Width, outbi.Size.Height), graphicsSystem.CreateRectangle(0, 0, outbi.Size.Width, outbi.Size.Height));
                    return (Chr: ch, bmp: outb);
                }).ToDictionary((item) => item.Chr, (item) => item.bmp)
                .ToDictionary((item) => item.Value, (item) => item.Key, new zcode_base.ZethanaFontCache.BitmapComparer());
            });
            var taskCombo = Task.WhenAll(task1, task2);
            taskCombo.Wait();
        }
        class BitmapComparer : IEqualityComparer<zcode_api_std.IBitmap>
        {
            public bool Equals(zcode_api_std.IBitmap x, zcode_api_std.IBitmap y)
            {
                if ((x != null) != (y != null))
                {
                    return false;
                }
                else if (x != null && y != null)
                {
                    if (ReferenceEquals(x, y))
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

            public int GetHashCode(zcode_api_std.IBitmap obj)
            {
                return 0;
            }
        }
    }
}
