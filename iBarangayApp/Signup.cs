using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Google.Android.Material.Snackbar;
using System;
using System.Collections.Specialized;
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

                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(edtEmail.WindowToken, 0);
                imm.HideSoftInputFromWindow(edtUsername.WindowToken, 0);
                imm.HideSoftInputFromWindow(edtPassword.WindowToken, 0);
                imm.HideSoftInputFromWindow(edtRpassword.WindowToken, 0);
                ValidateUsername(sender);
            }

        }

        private void ValidateUsername(object sender)
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                zsg_nameandimage name = new zsg_nameandimage();

                var uri = hosting.getCheckusername();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = edtUsername.Text;
                    datas["Email"] = edtEmail.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Username is Available")
                {
                    Info inf = new Info();
                    inf.Infos(edtEmail.Text, edtUsername.Text, edtPassword.Text);

                    StartActivity(new Intent(this, typeof(Signup2)));
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

        private void Back_Click(object sender, EventArgs e)
        {
            base.OnBackPressed();
        }


    }
}