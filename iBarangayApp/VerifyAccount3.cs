using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace iBarangayApp
{
    [Activity(Label = "Verification3")]
    public class VerifyAccount3 : Activity
    {
        private TextView tvBack;
        private Button btnSubmit;
        private EditText etCedulaNo;
        private ProgressBar pb;

        private string strImage2Url = "", strImage1Url;
        private zsg_nameandimage nme = new zsg_nameandimage();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VerifyAccount3);

            etCedulaNo = FindViewById<EditText>(Resource.Id.ETCedulaNo);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            tvBack = FindViewById<TextView>(Resource.Id.lblBack);
            pb = FindViewById<ProgressBar>(Resource.Id.pb);

            btnSubmit.Click += btnFinish_Click;
            tvBack.Clickable = true;
            tvBack.Click += delegate
            {
                OnBackPressed();
            };

            strImage1Url = Intent.GetStringExtra("image1");
            strImage2Url = Intent.GetStringExtra("image2");

            etCedulaNo.Text = nme.getCedulaNo();
        }

        private void btnFinish_Click(Object sender, EventArgs e)
        {
            if (etCedulaNo.Text == "" || etCedulaNo.Text == null)
            {
                etCedulaNo.Error = "Please enter a  valid Cedula No."; 
            }
            else
            {
                Verification();
            }
        }

        private async void Verification()
        {
            try
            {
                pb.Visibility = ViewStates.Visible;

                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getVerification();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = nme.getStrusername();
                    datas["IdImgUrl"] = strImage1Url;
                    datas["IdAndFaceImgUrl"] = strImage2Url;
                    datas["CedulaNo"] = etCedulaNo.Text;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Operation Failed")
                {
                    Toast.MakeText(this, "Something went wrong. Please try again later.", ToastLength.Short).Show();
                }
                else if (responseFromServer == "Operation Success")
                {

                    nme.nameandimage();

                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("Completed.");
                    alertDiag.SetMessage("Please wait atleast 3-7 days to validate your account.");
                    alertDiag.SetPositiveButton("Okay", (senderAlert, args) =>
                    {

                        StartActivity(new Intent(this, typeof(MainAnnouncement)).SetFlags(ActivityFlags.NoHistory));
                        Finish();
                        this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
                else
                {
                    Toast.MakeText(this, responseFromServer, ToastLength.Short).Show();
                }

            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }
            finally
            {
                pb.Visibility = ViewStates.Invisible;
            }
        }
    }
}