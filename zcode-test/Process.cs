public class Process
{
    public static void Main()
    {
        var fs = typeof(Process).Assembly.GetManifestResourceStream("zcode_test.rsrc.demo_text");
        var ts = new System.IO.StreamReader(fs);
        var msg = ts.ReadToEnd();
        
        var bm = zcode.ZethanaCode.FromText(msg);
        bm.Save("hw.png");
        var s = zcode.ZethanaCode.FromBitmap(bm);
        Console.WriteLine(s);
    }
}