﻿// See https://aka.ms/new-console-template for more information
using zcode_rsrcs;

Console.WriteLine("Hello, World!");
var g = new zcode_skia.SkiaGraphicsSystem();
var x = new zcode_base.ZethanaCode(g);
var fs = typeof(Resources).Assembly.GetManifestResourceStream(zcode_rsrcs.Resources.DemoResourceName);
var ts = new System.IO.StreamReader(fs);
var msg = ts.ReadToEnd();

var bm = x.FromText(msg);
bm.Save("hw.png");
var s = x.FromBitmap(bm);
Console.WriteLine(s);
var fo = new System.IO.FileStream("out.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
var fot = new System.IO.StreamWriter(fo);
fot.WriteLine(s);
fot.Close();
