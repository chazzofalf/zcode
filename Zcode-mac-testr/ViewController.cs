using System;

using AppKit;
using Foundation;
using zcode_rsrcs;

namespace Zcodemactestr
{
	public partial class ViewController : NSViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}
        private string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.!?";
        private bool hasChar = false;
        private char _char = '\0';
        private int charIndex = 0;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Do any additional setup after loading the view.
		}
        partial void doNext(NSObject sender)
        {
            if (hasChar)
            {
                charIndex++;
                if (charIndex >= chars.Length)
                {
                    charIndex = 0;
                }
            }
            else
            {
                hasChar = true;
            }
            _char = chars[charIndex];
            DisplayChar();
            
        }
        private void DisplayChar()
        {
            var g = new zcode_mac.MacGraphicsSystem();
            var x = new zcode_base.ZethanaCode(g);
            var bmt = $"{_char}";
            var bm = x.FromText(bmt);
        }
        partial void doPrev(NSObject sender)
        {
            if (hasChar)
            {
                charIndex--;
                if (charIndex < 0)
                {
                    charIndex = chars.Length-1;
                }
            }
            else
            {
                hasChar = true;
            }
            _char = chars[charIndex];
            DisplayChar();
        }
        partial void pushedMainButton(NSObject sender)
        {
            var g = new zcode_mac.MacGraphicsSystem();
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
        }
        public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
	}
}
