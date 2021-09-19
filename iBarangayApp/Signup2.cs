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
using static Android.App.DatePickerDialog;

namespace iBarangayApp
{
    [Activity(Label = "Signup2")]
    public class Signup2 : Activity, IOnDateSetListener
    {
        private Button btnRegistration, btnBirthDate, btnSignup;
        private ImageView imgProfile;
        private Spinner sprCivilStatus, sprGender, sprPurok, sprVoterStatus;
        private EditText ETFname, ETMname, ETLname, ETSname, ETBirthPlace, ETCedulaNo, ETContactNo;


        public static readonly int PickImageId = 1000;

        private const int DATE_DIALOG = 1, DATE_DIALOG1 = 2;
        private int year, month, day, year1, month1, day1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signup2);

            // Create your application here
            imgProfile = FindViewById<ImageView>(Resource.Id.img_Profile2);
            btnRegistration = FindViewById<Button>(Resource.Id.btnDateOfRegistration);
            btnBirthDate = FindViewById<Button>(Resource.Id.btnBirthDate);
            btnSignup = FindViewById<Button>(Resource.Id.btnsignupS2);

            ETFname = FindViewById<EditText>(Resource.Id.S2Fname);
            ETMname = FindViewById<EditText>(Resource.Id.S2Mname);
            ETLname = FindViewById<EditText>(Resource.Id.S2Lname);
            ETBirthPlace = FindViewById<EditText>(Resource.Id.S2Birthplace);
            ETCedulaNo = FindViewById<EditText>(Resource.Id.ETCedulaNo);
            ETContactNo = FindViewById<EditText>(Resource.Id.ETContactNo);

            imgProfile.Click += ImageClick;

            SetDate();
            btnBirthDate.Click += delegate {
                ShowDialog(DATE_DIALOG1);
            };

            btnRegistration.Click += delegate {
                ShowDialog(DATE_DIALOG);
            };

            btnSignup.Click += Signup_Click;
        }

        private void Signup_Click(object sender, EventArgs e)
        {
            if (ETFname.Text == "" || ETMname.Text == "" || ETLname.Text == "" || ETBirthPlace.Text == "" || ETCedulaNo.Text == "" || ETContactNo.Text == "")
            {
                Toast.MakeText(this, "Please Insert Valid Information.", ToastLength.Short).Show();
            }
        }

        private void ImageClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                Android.Net.Uri uri = data.Data;
                imgProfile.SetImageURI(uri);
            }
        }
        

        /// <summary>
        /// Date
        /// </summary>
        private void SetDate()
        {
            year = Int32.Parse(DateTime.Now.ToString("yyyy"));
            month = Int32.Parse(DateTime.Now.ToString("MM"));
            day = Int32.Parse(DateTime.Now.ToString("dd"));

            year1 = Int32.Parse(DateTime.Now.ToString("yyyy"));
            month1 = Int32.Parse(DateTime.Now.ToString("MM"));
            day1 = Int32.Parse(DateTime.Now.ToString("dd"));
        }

        int SelectedNum=0;
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, year, month, day);
                    }
                    break;
                case DATE_DIALOG1:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, year1, month1, day1);
                    }
                    break;
                default:
                    break;
            }

            return base.OnCreateDialog(id);
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {

            if (SelectedNum == 1) {
                this.year = year;
                this.month = month;
                day = dayOfMonth;

                DateTime date = new DateTime(year, month, day);
                btnRegistration.Text = date.ToString("MMM dd, yyyy");
            }
            else
            {
                this.year1 = year;
                this.month1 = month;
                day1 = dayOfMonth;

                DateTime date1 = new DateTime(year1, month1, day1);
                btnBirthDate.Text = date1.ToString("MMM dd, yyyy");
            }
        }
    }
}