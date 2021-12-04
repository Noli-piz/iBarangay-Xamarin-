using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Widget;
using Org.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Timers;

namespace iBarangayApp
{
    [Activity(Label = "Signup1")]
    public class Signup1 : Activity
    {
        private Button btnSubmit;
        private EditText etNum1, etNum2, etNum3, etNum4, etNum5, etNum6;
        private TextView tvResend, tvEmail;
        private zsg_randomnum randomNumber = new zsg_randomnum();
        private Timer _timer;

        private int tries = 3;
        private int mins, secs;
        private Info inf = new Info();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup1);

            etNum1 = FindViewById<EditText>(Resource.Id.etNum1);
            etNum2 = FindViewById<EditText>(Resource.Id.etNum2);
            etNum3 = FindViewById<EditText>(Resource.Id.etNum3);
            etNum4 = FindViewById<EditText>(Resource.Id.etNum4);
            etNum5 = FindViewById<EditText>(Resource.Id.etNum5);
            etNum6 = FindViewById<EditText>(Resource.Id.etNum6);
            tvResend = FindViewById<TextView>(Resource.Id.tvResend);
            tvEmail = FindViewById<TextView>(Resource.Id.tvEmail);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            btnSubmit.Click += BtnSubmit_Click;
            tvResend.Clickable = true;
            tvResend.Click += delegate
            {
                randomNumber = new zsg_randomnum();
                SendEmailAsync(randomNumber.randomNum());

                mins = 2;
                secs = 59;
                _timer = new System.Timers.Timer();
                _timer.Interval = 1000;
                _timer.Elapsed += OnTimedEvent;
                _timer.Enabled = true;
                tvResend.Clickable = false;
            };

            SendEmailAsync(randomNumber.randomNum());

            string editemail = inf.getStrEmail();
            string pattern = @"(?<=[\w]{4})[\w-\._\+%]*(?=[\w]{2}@)";
            string edittedemail = Regex.Replace(editemail, pattern, m => new string('*', m.Length));

            tvEmail.Text =" "+edittedemail;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string numText = etNum1.Text + etNum2.Text + etNum3.Text + etNum4.Text + etNum5.Text + etNum6.Text;
            if (etNum1.Text == "")
            {
                etNum1.Error = "Cannot be empty!";
            }
            else if (etNum2.Text == "")
            {
                etNum2.Error = "Cannot be empty!";
            }
            else if (etNum3.Text == "")
            {
                etNum3.Error = "Cannot be empty!";
            }
            else if (etNum4.Text == "")
            {
                etNum4.Error = "Cannot be empty!";
            }
            else if (etNum5.Text == "")
            {
                etNum5.Error = "Cannot be empty!";
            }
            else if (etNum6.Text == "")
            {
                etNum6.Error = "Cannot be empty!";
            }
            else if (numText == randomNumber.randomNum())
            {
                StartActivity(new Intent(this, typeof(Signup2)));
                Finish();
            }
            else
            {
                tries--;
                Toast.MakeText(this, "Verifcation Code is Wrong!", ToastLength.Short).Show();

                if (tries==0)
                {

                }
            }
        }

        public async void SendEmailAsync(string randomNumber)
        {
            zsg_ApiKey ApiKey = new zsg_ApiKey();
            ApiKey.loadKeys();

            var client = new SendGridClient(ApiKey.getSendGridKey());
            var msg = new SendGridMessage()
            {

                From = new EmailAddress(ApiKey.getSendGridEmail(), "iBarangay<no-reply>"),
                Subject = "Verification Code",
                PlainTextContent = "Your verification code is: " + randomNumber,
                HtmlContent = "<p>Your verification code is: <strong> " + randomNumber + "</strong></p>"
            };

            msg.AddTo(new EmailAddress(inf.getStrEmail(), "ibarangay-user"));
            var response = await client.SendEmailAsync(msg);
            Android.Util.Log.Error("ERROR:" , response.StatusCode +"" );
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            secs--;

            if (secs == 0)
            {
                if (mins == 0)
                {
                    tvResend.Text = "Resend Code?";
                    _timer.Stop();
                    tvResend.Clickable = true;
                }
                else
                {
                    mins--;
                    secs = 59;
                }
            }

            string strmins = "0" + mins ;
            string strsecs = secs.ToString().Length <=1 ? "0" + secs : secs +"" ;

            tvResend.Text = "Resend OTP in " + strmins + ":" + strsecs;
        }

    }
}