// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Zcodemactestr
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSImageView charView { get; set; }

		[Action ("doNext:")]
		partial void doNext (Foundation.NSObject sender);

		[Action ("doPrev:")]
		partial void doPrev (Foundation.NSObject sender);

		[Action ("pushedMainButton:")]
		partial void pushedMainButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (charView != null) {
				charView.Dispose ();
				charView = null;
			}
		}
	}
}
