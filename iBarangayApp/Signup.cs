using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Google.Android.Material.Snackbar;
using Org.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
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

            RetrieveEmail();
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
                if (checkEmail(edtEmail.Text)) {
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(edtEmail.WindowToken, 0);
                    imm.HideSoftInputFromWindow(edtUsername.WindowToken, 0);
                    imm.HideSoftInputFromWindow(edtPassword.WindowToken, 0);
                    imm.HideSoftInputFromWindow(edtRpassword.WindowToken, 0);
                    ValidateUsername(sender);
                }
                else
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("Not Valid Email.");
                    alertDiag.SetMessage("Please enter a valid email.");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        alertDiag.Dispose();
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }

        }

        private bool checkEmail(String email)
        {
            try
            {
                var add = new System.Net.Mail.MailAddress(email);
                return add.Address == email;
            }
            catch (Exception es)
            {
                return false;
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

                    StartActivity(new Intent(this, typeof(Signup1)));
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

        async void RetrieveEmail()
        {
            using (var client = new HttpClient())
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getApiKey();
                var result = await client.GetStringAsync(uri);

                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray information = jsonresult.GetJSONArray("info");
                    JSONObject info = information.GetJSONObject(0);

                    zsg_ApiKey ap = new zsg_ApiKey();
                    ap.setSendGridKey(info.GetString("ApiKey"));
                }
            }
        }
    }
}