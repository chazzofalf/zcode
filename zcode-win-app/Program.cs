using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zcode_base;
using zcode_rsrcs;
using zcode_win;

namespace zcode_win_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gs = new WindowsGraphicsSystem();
            var proc = new ZethanaCode(gs);
            var fs = typeof(Resources).Assembly.GetManifestResourceStream(zcode_rsrcs.Resources.DemoResourceName);
            var ts = new System.IO.StreamReader(fs);
            var msg = ts.ReadToEnd();

            var bm = proc.FromText(msg);
            bm.Save("hw.png");
            var s = proc.FromBitmap(bm);
            Console.WriteLine(s);
            var fo = new System.IO.FileStream("out.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
            var fot = new System.IO.StreamWriter(fo);
            fot.WriteLine(s);
            fot.Close();

        }
    }
}
