using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace iBarangayApp
{
    [Activity(Label = "Signup")]
    public class Signup : Activity
    {
        private EditText edtUsername, edtPassword, edtEmail, edtRpassword;
        private Button btnNext;
        private TextView tvBack;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup);

            // Create your application here
            edtEmail = FindViewById<EditText>(Resource.Id.ETemail);
            edtUsername = FindViewById<EditText>(Resource.Id.ETusername);
            edtPassword = FindViewById<EditText>(Resource.Id.ETpassword);
            edtRpassword = FindViewById<EditText>(Resource.Id.ETRpassword);
            btnNext = FindViewById<Button>(Resource.Id.btnNext);
            tvBack = FindViewById<TextView>(Resource.Id.lblBack);

            tvBack.Click += Back_Click;
            btnNext.Click += BtnNext_Click;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (edtEmail.Text == "" || edtUsername.Text == "" || edtPassword.Text == "" || edtRpassword.Text == "")
            {
                Toast.MakeText(Application.Context, "Please Fill up all fields.", ToastLength.Short).Show();
            }
            else if (edtPassword.Text != edtRpassword.Text)
            {
                Toast.MakeText(Application.Context, "Password and Retry Password is not equal.", ToastLength.Short).Show();
            }
            else
            {
                ValidateUsername(sender);
            }

        }

        private void ValidateUsername(object sender)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://192.168.254.114/iBarangay/ibarangay_checkusername.php?Username=" + edtUsername.Text);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                var responseFromServer = reader.ReadToEnd();

                if (responseFromServer == "Username already Exist!")
                {

                    View view = (View)sender;
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
                else
                {
                    Info inf = new Info();
                    inf.Infos(edtEmail.Text, edtUsername.Text, edtPassword.Text);

                    Intent intent = new Intent(this, typeof(Signup2));
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Please check your connection.", ToastLength.Short).Show();
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Login)).SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);
            Finish();
        }


    }
}