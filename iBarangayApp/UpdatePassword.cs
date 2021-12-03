using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iBarangayApp
{
    [Activity(Label = "UpdatePassword")]
    public class UpdatePassword : Activity
    {
        private EditText etPass, etConPass;
        private Button btnSubmit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UpdatePassword);

            etPass = FindViewById<EditText>(Resource.Id.ETpassword);
            etConPass = FindViewById<EditText>(Resource.Id.ETRpassword);

            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (etPass.Text == "")
            {
                etPass.Error = "Cannot be empty!";
            }
            else if (etConPass.Text == "")
            {
                etConPass.Error = "Cannot be empty!";
            }
            else if (etPass.Text != etConPass.Text)
            {
                Toast.MakeText(this, "Password is not equal to Confirm Password", ToastLength.Short).Show();
            }
            else
            {
                //StartActivity(new Intent(this, typeof(UpdatePassword)));
                updatePass();
            }
        }

        private async void updatePass()
        {
            try
            {
                zsg_hosting hosting = new zsg_hosting();
                zsg_emailverfication email = new zsg_emailverfication();

                var uri = hosting.getUpdatePass();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Email"] = email.getEmail();
                    datas["Password"] = etPass.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Updated Successfully")
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetCancelable(false);
                    alertDiag.SetTitle("Password Successfuly Updated");
                    alertDiag.SetMessage("Don't forget your password. Ok?");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        StartActivity(new Intent(this, typeof(Login)).SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask));
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();

                }
                else
                {
                    Toast.MakeText(this, responseFromServer, ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Please check your connection.", ToastLength.Short).Show();
            }
        }
    }
}