using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Timers;

namespace iBarangayApp
{
    [Activity(Label = "ForgetPassword2")]
    public class ForgetPassword2 : Activity
    {
        private Button btnSubmit;
        private EditText etNum1, etNum2, etNum3, etNum4, etNum5, etNum6;
        private TextView tvResend;
        private zsg_randomnum randomNumber = new zsg_randomnum();
        private Timer _timer;

        private int tries = 3;
        private int mins, secs;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ForgetPassword2);

            etNum1 = FindViewById<EditText>(Resource.Id.etNum1);
            etNum2 = FindViewById<EditText>(Resource.Id.etNum2);
            etNum3 = FindViewById<EditText>(Resource.Id.etNum3);
            etNum4 = FindViewById<EditText>(Resource.Id.etNum4);
            etNum5 = FindViewById<EditText>(Resource.Id.etNum5);
            etNum6 = FindViewById<EditText>(Resource.Id.etNum6);
            tvResend = FindViewById<TextView>(Resource.Id.tvResend);
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
                Toast.MakeText(this, "Verifcation Code is Wrong! You only have " + tries + " left.", ToastLength.Short).Show();
            }
        }

        public async void SendEmailAsync(string randomNumber)
        {

            zsg_emailverfication email = new zsg_emailverfication();

            csApiKey ApiKey = new csApiKey();
            ApiKey.loadKeys();

            var client = new SendGridClient(ApiKey.getSendGridKey());
            var msg = new SendGridMessage()
            {

                From = new EmailAddress(ApiKey.getSendGridEmail(), "iBarangay<no-reply>"),
                Subject = "Verification Code",
                PlainTextContent = "Your verification code is: " + randomNumber,
                HtmlContent = "<p>Your verification code is: <strong> " + randomNumber + "</strong></p>"
            };

            msg.AddTo(new EmailAddress(email.getEmail(), "ibarangay-user"));
            var response = await client.SendEmailAsync(msg);
            btnSubmit.Text = response.StatusCode.ToString();
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            secs--;

            if (secs == 0)
            {
                if (mins == 0)
                {
                    _timer.Stop();
                    tvResend.Clickable = true;
                    tvResend.Text = "Resend Code";
                }
                else
                {
                    mins--;
                    secs = 59;
                }
            }

            tvResend.Text = mins + " " + secs;
        }
    }
}