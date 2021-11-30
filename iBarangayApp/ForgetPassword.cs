using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace iBarangayApp
{
    //[Activity(Label = "ForgetPassword", MainLauncher = true)]
    [Activity(Label = "ForgetPassword")]
    public class ForgetPassword : Activity
    {
        private Button btnSubmit;
        private EditText etEmail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgetPassword);

            etEmail = FindViewById<EditText>(Resource.Id.ETemail);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (etEmail.Text == "")
            {
                etEmail.Error = "Cannot be empty!";
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
                zsg_hosting hosting = new zsg_hosting();
                zsg_nameandimage name = new zsg_nameandimage();

                var uri = hosting.getForgotPass();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Email"] = etEmail.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Email is Exist")
                {
                    zsg_emailverfication email = new zsg_emailverfication();
                    email.setEmail(etEmail.Text);

                    StartActivity(new Intent(this, typeof(ForgetPassword2)));
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
        }

    }
}