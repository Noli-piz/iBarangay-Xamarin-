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
                Intent intent = new Intent(this, typeof(Signup2));
                StartActivity(intent);
            }
            
        }
    }
}