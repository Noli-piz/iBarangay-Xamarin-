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

namespace iBarangayApp
{
    //[Activity(Label = "Login")]
    [Activity(Label = "IBarangay", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    public class Login : Activity
    {
        private EditText edtUsername, edtPassword;
        private Button btnLogin, btnSignup;

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
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (edtUsername.Text== "" || edtPassword.Text == "")
            {
                Toast.MakeText(Application.Context, "Please enter Username or Password!", ToastLength.Short).Show();
            }
            else
            {
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

            WebRequest request = WebRequest.Create("http://192.168.254.114/iBarangay/ibarangay_login.php?Username="+ edtUsername.Text  +"&Password="+ edtPassword.Text);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            
            if(responseFromServer == "Login Success")
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else
            {
                View view = (View)sender;
                Snackbar.Make(view, responseFromServer , Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }

        }
    
    }


}

