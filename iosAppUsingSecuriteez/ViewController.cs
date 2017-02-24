using System;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Auth;

namespace iosAppUsingSecuriteez
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        async partial void TheButton_TouchUpInside(UIButton sender)
        {
            //var http = new HttpClient();
            //var output = await http.GetStringAsync("http://192.168.7.35/WebApiUsingSecuriteez/api/Customers");
            //theOutput.Text = output;

            var AuthTaskSource = new TaskCompletionSource<Account>();

            var authenticator = new OAuth2Authenticator(
                "ios",
                "SiteAccess",
                new Uri("http://192.168.7.38/WebSiteWithSecuriteez/auth/connect/authorize"),
                new Uri("http://www.xamarin.com")
                );
            
            var authGui = authenticator.GetUI();

            authenticator.Completed += (obj, args) =>
            {
                AuthTaskSource.SetResult(args.Account);
            };
            authenticator.Error += (obj, args) =>
            {
                if (args.Exception != null) AuthTaskSource.SetException(args.Exception);
                else AuthTaskSource.SetException(new ApplicationException(args.Message));
            };

            this.PresentViewController(authGui, true, null);

            Account account = null;
            try
            {
                account = await AuthTaskSource.Task;
            }
            catch (Exception e)
            {
                theOutput.Text = $"ERROR: {e.Message}";
            }
            finally
            {
                authGui.DismissViewController(true, null);
            }

            theOutput.Text = "Calling Svc";
            var req = new CustomOAuth2Request("GET", new Uri("http://192.168.7.38/WebApiUsingSecuriteez/api/Customers"), null, account, true);

            theOutput.Text = await (await req.GetResponseAsync()).GetResponseTextAsync();


        }
    }
}