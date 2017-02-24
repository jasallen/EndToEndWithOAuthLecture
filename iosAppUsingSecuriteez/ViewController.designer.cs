// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iosAppUsingSecuriteez
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton theButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel theOutput { get; set; }

        [Action ("TheButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TheButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (theButton != null) {
                theButton.Dispose ();
                theButton = null;
            }

            if (theOutput != null) {
                theOutput.Dispose ();
                theOutput = null;
            }
        }
    }
}