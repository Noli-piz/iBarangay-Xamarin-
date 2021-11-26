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
using SendGrid;
using SendGrid.Helpers.Mail;
using Google.Android.Material.Snackbar;
using System.Collections.Specialized;
using System.Net;

namespace iBarangayApp
{
    [Activity(Label = "ForgetPassword2")]
    public class ForgetPassword2 : Activity
    {
        private Button btnSubmit;
        private EditText etNum1, etNum2, etNum3, etNum4, etNum5, etNum6;
        private TextView tvResend;
        private zsg_randomnum randomNumber =  new zsg_randomnum();

        private int tries=3;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgetPassword2);

            etNum1 =  FindViewById<EditText>(Resource.Id.etNum1);
            etNum2 =  FindViewById<EditText>(Resource.Id.etNum2);
            etNum3 =  FindViewById<EditText>(Resource.Id.etNum3);
            etNum4 =  FindViewById<EditText>(Resource.Id.etNum4);
            etNum5 =  FindViewById<EditText>(Resource.Id.etNum5);
            etNum6 =  FindViewById<EditText>(Resource.Id.etNum6);
            tvResend =  FindViewById<TextView>(Resource.Id.tvResend);
            btnSubmit =  FindViewById<Button>(Resource.Id.btnSubmit);

            btnSubmit.Click += BtnSubmit_Click;
            tvResend.Clickable = true;
            tvResend.Click += delegate{
                randomNumber = new zsg_randomnum();
                SendEmailAsync(randomNumber.randomNum());
            };

            SendEmailAsync(randomNumber.randomNum());
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string numText = etNum1.Text + etNum2.Text + etNum3.Text + etNum4.Text + etNum5.Text + etNum6.Text;
            if (etNum1.Text == "")
            {
                etNum1.Error = "Cannot be empty!";
            }
            else if (numText == randomNumber.randomNum())
            {
                StartActivity(new Intent(this, typeof(UpdatePassword)));
            }
            else
            {
                tries--;
                Toast.MakeText(this, "Verifcation Code is Wrong! You only have " + tries +" left.", ToastLength.Short).Show();
            }
        }

         public async void SendEmailAsync(string randomNumber){

            zsg_emailverfication email = new zsg_emailverfication();

            csApiKey ApiKey = new csApiKey();
            ApiKey.loadKeys();

            var client = new SendGridClient(ApiKey.getSendGridKey());
            var msg = new SendGridMessage()
            {

                From = new EmailAddress(ApiKey.getSendGridEmail(), "iBarangay<no-reply>"),
                Subject = "Verification Code",
                PlainTextContent = "Your verification code is: " + randomNumber,
                HtmlContent = "<p>Your verification code is: <strong> " + randomNumber +"</strong></p>"
            };

            msg.AddTo(new EmailAddress(email.getEmail(), "Test-user"));
            var response = await client.SendEmailAsync(msg);
        }
    }
}