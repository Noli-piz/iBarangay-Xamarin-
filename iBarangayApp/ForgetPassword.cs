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
    //[Activity(Label = "ForgetPassword", MainLauncher = true)]
    [Activity(Label = "ForgetPassword")]
    public class ForgetPassword : Activity
    {
        private Button btnSubmit;
        private EditText etUsername;
        private zsg_emailverfication email = new zsg_emailverfication();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgetPassword);

            etUsername = FindViewById<EditText>(Resource.Id.ETusername);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (etUsername.Text == "")
            {
                etUsername.Error = "Cannot be empty!";
            }
            else
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(etUsername.WindowToken, 0);

                ValidateUsername(sender);
            }
        }


        private void ValidateUsername(object sender)
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();

                var uri = hosting.getForgotPass();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = etUsername.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Username is Exist")
                {
                    RetrieveEmail();
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


        async void RetrieveEmail()
        {
            using (var client = new HttpClient())
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getEmail() + "?Username=" + etUsername.Text;
                var result = await client.GetStringAsync(uri);

                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray information = jsonresult.GetJSONArray("info");
                    JSONObject info = information.GetJSONObject(0);

                    email.setEmail(info.GetString("Email"));

                    //zsg_ApiKey ap = new zsg_ApiKey();
                    //ap.setSendGridKey(info.GetString("ApiKey"));

                    StartActivity(new Intent(this, typeof(ForgetPassword2)));
                }

            }
        }
    }
}