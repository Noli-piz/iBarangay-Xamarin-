using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Net;
using System.Net;
using Java.Util.Concurrent;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using Google.Android.Material.Snackbar;
using System.Collections.Specialized;
using Google.Android.Material.ProgressIndicator;
using System.Threading.Tasks;

namespace iBarangayApp
{
    //[Activity(Label = "Login")]
    [Activity(Label = "IBarangay", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    public class Login : Activity
    {
        private EditText edtUsername, edtPassword;
        private Button btnLogin, btnSignup;
        private CircularProgressIndicator progBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            // Create your application here
            edtUsername = FindViewById<EditText>(Resource.Id.ETusername);
            edtPassword = FindViewById<EditText>(Resource.Id.ETpassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnlogin);
            btnSignup = FindViewById<Button>(Resource.Id.btnsignupS2);

            btnLogin.Click += BtnLogin_Click;
            btnSignup.Click += BtnSignup_Click;

            progBar = FindViewById<CircularProgressIndicator>(Resource.Id.circular);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (edtUsername.Text== "" || edtPassword.Text == "")
            {
                Toast.MakeText(Application.Context, "Please enter Username or Password!", ToastLength.Short).Show();
            }
            else
            {
                progBar.Visibility = ViewStates.Visible;
                GetInfo(sender);
            }
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Signup));
            StartActivity(intent);
        }

        private async void GetInfo(object sender)
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getLogin();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = edtUsername.Text;
                    datas["Password"] = edtPassword.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Login Success")
                {
                    zsg_nameandimage user = new zsg_nameandimage();
                    user.setStrusername(edtUsername.Text);
                    user.nameandimage();

                    await Task.Delay(4000);
                    Intent intent = new Intent(this, typeof(MainAnnouncement));
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    View view = (View)sender;
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Please check your connection.", ToastLength.Short).Show();
            }
            finally
            {
                progBar.Visibility = ViewStates.Invisible;
            }
        }
    
    }


}

