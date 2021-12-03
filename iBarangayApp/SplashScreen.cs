using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.ProgressIndicator;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iBarangayApp
{
    //[Activity(Label = "SplashScreen", NoHistory =true)]
    [Activity(Label = "iBarangay", NoHistory =true, MainLauncher = true)]
    public class SplashScreen : Activity
    {

        private ISharedPreferences pref;
        private CircularProgressIndicator progBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splashscreen);

            progBar = FindViewById<CircularProgressIndicator>(Resource.Id.circular);
        }

        protected override void OnResume()
        {
            base.OnResume();

            pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
            String strLogin = pref.GetString("Logout", String.Empty);
            if (strLogin == "false")
            {
                string username = pref.GetString("Username", String.Empty);
                string password = pref.GetString("Password", String.Empty);

                GetInfo(username, password);
            }
            else
            {
                Task startupWork = new Task(() => { SimulateStartup(); });
                startupWork.Start();
            }
        }

        async void SimulateStartup()
        {
            await Task.Delay(1000);
            StartActivity(new Intent(Application.Context, typeof(Login)));
        }


        private async void GetInfo(string username, string password)
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getLogin();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = username;
                    datas["Password"] = password;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Login Success")
                {
                    zsg_nameandimage user = new zsg_nameandimage();
                    user.setStrusername(username);
                    user.nameandimage();


                    await Task.Delay(4000);
                    Intent intent = new Intent(this, typeof(MainAnnouncement));
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Snackbar.Make(FindViewById(Resource.Id.llayout), responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Please check your connection.", ToastLength.Short).Show();
            }
            finally
            {

            }
        }

    }
}