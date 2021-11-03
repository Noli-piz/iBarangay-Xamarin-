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
using Google.Android.Material.TextField;
using Android.Views.InputMethods;
using Android.Util;
using Android.Gms.Common;
using Firebase.Iid;
using Plugin.FirebasePushNotification;
using Firebase.Messaging;

namespace iBarangayApp
{
    //[Activity(Label = "Login")]
    [Activity(Label = "IBarangay", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    public class Login : Activity
    {
        private EditText edtUsername, edtPassword;
        private Button btnLogin, btnSignup;
        private CircularProgressIndicator progBar;

        private ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

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
            zsg_nameandimage user = new zsg_nameandimage();
            user.reset();


            String strLogin = pref.GetString("Logout", String.Empty);
            if (strLogin == "false")
            {
                edtUsername.Text = pref.GetString("Username", String.Empty);
                edtPassword.Text = pref.GetString("Password", String.Empty);
                Object sender = new object();

                Disable();
                GetInfo(sender);
            }

            IsPlayServiceAvailable();
            Log.Error("TOKEN", FirebaseInstanceId.Instance.Token);
            FirebaseMessaging.Instance.SubscribeToTopic("ibarangay");
        }

        TextInputLayout lay;
        bool visibleToUser =true;
        private void Disable()
        {
            edtUsername.Visibility = ViewStates.Invisible;
            edtPassword.Visibility = ViewStates.Invisible;
            btnLogin.Visibility = ViewStates.Invisible;
            btnSignup.Visibility = ViewStates.Invisible;

            lay = FindViewById<TextInputLayout>(Resource.Id.txtLayout1S2);
            lay.Visibility = ViewStates.Invisible;
            visibleToUser = false;
        }
        private void Enable()
        {
            edtUsername.Visibility = ViewStates.Visible;
            edtPassword.Visibility = ViewStates.Visible;
            btnLogin.Visibility = ViewStates.Visible;
            btnSignup.Visibility = ViewStates.Visible;

            lay.Visibility = ViewStates.Visible;
            visibleToUser = true;
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (edtUsername.Text== "" || edtPassword.Text == "")
            {
                Toast.MakeText(Application.Context, "Please enter Username or Password!", ToastLength.Short).Show();
            }
            else
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(edtUsername.WindowToken, 0);
                imm.HideSoftInputFromWindow(edtPassword.WindowToken, 0);

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

                progBar.Visibility = ViewStates.Visible;
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
                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.PutString("Username", edtUsername.Text);
                    edit.PutString("Password", edtPassword.Text);
                    edit.PutString("Logout", "false");
                    edit.Apply();

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
                if (visibleToUser == false) {
                    Enable();
                }
            }
        }


        //// Push Notification from Firebase
        ///

        private bool IsPlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(this, GoogleApiAvailability.Instance.GetErrorString(resultCode).ToString(), ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "This device is not supported", ToastLength.Short).Show();
                }
                return false;
            }
            else
            {
                Toast.MakeText(this, "Google Play Service is available", ToastLength.Short).Show();
                return true;
            }
        }
    }
}

