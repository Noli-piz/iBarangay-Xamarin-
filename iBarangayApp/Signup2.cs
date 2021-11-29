using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO; 
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using static Android.App.DatePickerDialog;
using System.Threading.Tasks;

namespace iBarangayApp
{
    [Activity(Label = "Signup2")]
    public class Signup2 : Activity, IOnDateSetListener
    {
        private Button btnRegistration, btnBirthDate, btnSignup;
        private ImageView imgProfile;
        private Spinner sprCivilStatus, sprGender, sprPurok, sprVoterStatus;
        private EditText ETFname, ETMname, ETLname, ETSname, ETBirthPlace, ETCedulaNo, ETContactNo;
        private TextView tvBack;
        private ProgressBar pb;

        public static readonly int PickImageId = 1000;

        private const int DATE_DIALOG = 1, DATE_DIALOG1 = 2;
        private int year, month, day, year1, month1, day1;

        private string strEmail, strPassword, strImageUrl="";
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
            ETSname = FindViewById<EditText>(Resource.Id.S2Sname);
            ETBirthPlace = FindViewById<EditText>(Resource.Id.S2Birthplace);
            ETCedulaNo = FindViewById<EditText>(Resource.Id.ETCedulaNo);
            ETContactNo = FindViewById<EditText>(Resource.Id.ETContactNo);

            sprCivilStatus = FindViewById<Spinner>(Resource.Id.spnrCivilStatus);
            sprGender = FindViewById<Spinner>(Resource.Id.spnrGender);
            sprPurok = FindViewById<Spinner>(Resource.Id.spnrPurok);
            sprVoterStatus = FindViewById<Spinner>(Resource.Id.spnrVoterStatus);

            tvBack = FindViewById<TextView>(Resource.Id.lblBack2);
            pb = FindViewById<ProgressBar>(Resource.Id.pb);

            LoadSpinners();

            imgProfile.Click += ImageClick;

            btnBirthDate.Click += delegate {
                ShowDialog(DATE_DIALOG1);
            };

            btnRegistration.Click += delegate {
                ShowDialog(DATE_DIALOG);
            };

            SetDate();
            btnSignup.Click += Signup_Click;

            sprCivilStatus.ItemSelected += spCV_Click;
            sprGender.ItemSelected += spG_Click;
            sprPurok.ItemSelected += spPRK_Click;
            sprVoterStatus.ItemSelected += spVS_Click;

            tvBack.Click += tvBack_Click;
        }

        /// Spinner Event
        /// 

        private String strCV, strGDR, strPRK, strVS;
        private void spCV_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strCV = civil[e.Position].ToString();
        }

        private void spG_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strGDR = gender[e.Position].ToString();
        }

        private void spPRK_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strPRK = purok[e.Position].ToString();
        }

        private void spVS_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strVS = opt[e.Position].ToString();
        }

        /// <summary>
        private void Signup_Click(object sender, EventArgs e)
        {
            if (ETFname.Text == ""){
                ETFname.Error = "Cannot be Empty";
            }
            else if (ETLname.Text == "") {
                ETLname.Error = "Cannot be Empty";
            }
            else if (ETBirthPlace.Text == "") { 
                ETBirthPlace.Error = "Cannot be Empty";
            }
            else if ( ETContactNo.Text == ""){
                ETContactNo.Error = "Cannot be Empty";
            }
            else if (strCV == "" || strGDR == "" || strPRK=="" || strVS=="")
            {
                ((TextView)sprCivilStatus.SelectedView).Error = "None Selected";
                Toast.MakeText(this, "Please Select Valid Information.", ToastLength.Short).Show();
            } else if (strImageUrl=="")
            {
                Toast.MakeText(this, "Please Select an Image.", ToastLength.Short).Show();
            }
            else
            {
                InsertInformation(sender);
            }
        }

        private async void InsertInformation(object sender)
        {
            Info inf = new Info();

            String[] field = new String[18];
            field[0] = "Username";
            field[1] = "Password";
            field[2] = "Status";
            field[3] = "Fname";
            field[4] = "Mname";
            field[5] = "Lname";
            field[6] = "Sname";
            field[7] = "Birthplace";
            field[8] = "Birthdate";
            field[9] = "CivilStatus";
            field[10] = "Gender";
            field[11] = "id_purok";
            field[12] = "VoterStatus";
            field[13] = "DateOfRegistration";
            field[14] = "ContactNo";
            field[15] = "CedulaNo";
            field[16] = "Email";
            field[17] = "Image";

            String[] data = new String[18];
            data[0] = inf.getStrUsername();
            data[1] = inf.getStrPassword();
            data[2] = "False";
            data[3] = ETFname.Text;
            data[4] = ETMname.Text=="" ? "NONE" : ETMname.Text;
            data[5] = ETLname.Text;
            data[6] = ETSname.Text=="" ? "NONE" : ETSname.Text;
            data[7] = ETBirthPlace.Text;
            data[8] = Bdate;
            data[9] = strCV;
            data[10] = strGDR;
            data[11] = strPRK;
            data[12] = strVS;
            data[13] = Rdate;
            data[14] = ETContactNo.Text;
            data[15] = ETCedulaNo.Text == "" ? "NONE" : ETCedulaNo.Text;
            data[16] = inf.getStrEmail();
            data[17] = strImageUrl;

            
            zsg_hosting hosting = new zsg_hosting();
            var uri = hosting.getSignup3();

            try
            {
                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    for (int i=0; i< field.Length; i++)
                    {
                        datas[field[i].ToString()] = data[i].ToString();
                    }

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Sign up Failed")
                {

                    View view = (View)sender;
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
                else if(responseFromServer == "Sign Up Success")
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetCancelable(false);
                    alertDiag.SetTitle("Sign Up Success.");
                    alertDiag.SetMessage("You Successfuly Create New Account.");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) => {

                        StartActivity(new Intent(this, typeof(Login)).SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask));
                        Finish();
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();
                    await Task.CompletedTask;

                }
                else
                {
                    View view = (View)sender;
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
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
                Android.Net.Uri filePath = data.Data;
                try
                {
                    if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.P )
                    {
                        Bitmap mBitMap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                        imgProfile.SetImageBitmap(mBitMap);
                        byte[] bitmapData;
                        using (var stream = new MemoryStream())
                        {
                            mBitMap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            bitmapData = stream.ToArray();
                        }
                        MemoryStream inputStream = new MemoryStream(bitmapData);
                        Upload(inputStream);
                    }
                    else
                    {
                        var source = ImageDecoder.CreateSource(ContentResolver, filePath);
                        var MBitmap = ImageDecoder.DecodeBitmap(source);
                        imgProfile.SetImageBitmap(MBitmap);

                        byte[] bitmapData;
                        using (var stream = new MemoryStream())
                        {
                            MBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            bitmapData = stream.ToArray();
                        }
                        MemoryStream inputStream = new MemoryStream(bitmapData);
                        Upload(inputStream);
                    }
                }
                catch (IOException e)
                {
                    Toast.MakeText(this, "" + e.ToString(), ToastLength.Short).Show();
                }

            }
        }

        /// upload Blob function
        private async void Upload(Stream stream)
        {
            try
            {
                pb.Visibility = ViewStates.Visible;
                btnSignup.Enabled = false;

                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("profileimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImageUrl = URL;
                Toast.MakeText(this, "Image uploaded to Blob Storage Successfully!", ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "" + e.ToString(), ToastLength.Short).Show();
            }
            finally
            {
                pb.Visibility = ViewStates.Invisible;
                btnSignup.Enabled = true;
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
            month1 = Int32.Parse(DateTime.Now.ToString("MM")) ;
            day1 = Int32.Parse(DateTime.Now.ToString("dd"));

            DateTime date = new DateTime(year, month, day);
            DateTime date1 = new DateTime(year1, month1, day1);
            
            Rdate = date.ToString("yyyy-MM-dd");
            Bdate = date1.ToString("yyyy-MM-dd");

            btnRegistration.Text = date.ToString("MMM dd, yyyy");
            btnBirthDate.Text = date1.ToString("MMM dd, yyyy");

        }

        int SelectedNum=0;
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, year, month-1, day);
                    }
                case DATE_DIALOG1:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, year1, month1-1, day1);
                    }
                default:
                    break;
            }

            return base.OnCreateDialog(id);
        }

        private String Bdate, Rdate;
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {

            if (SelectedNum == 1) {
                this.year = year;
                this.month = month+1;
                day = dayOfMonth;

                DateTime date = new DateTime(this.year, this.month, day);
                btnRegistration.Text = date.ToString("MMMM dd, yyyy");
                Rdate = date.ToString("yyyy-MM-dd");
            }
            else
            {
                this.year1 = year;
                this.month1 = month+1;
                day1 = dayOfMonth;

                DateTime date1 = new DateTime(year1, month1, day1);
                btnBirthDate.Text = date1.ToString("MMMM dd, yyyy");
                Bdate = date1.ToString("yyyy-MM-dd");
            }
        }


        private List<string> civil = new List<string>(), gender= new List<string>(), purok = new List<string>(), opt = new List<string>();
        private async void LoadSpinners()
        {
            //// Civil Status 
            ///

            zsg_hosting hosting = new zsg_hosting();
            using (var client = new HttpClient())
            {
                var uri = hosting.getCivilstatus();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray cvl = jsonresult.GetJSONArray("civilstatus");

                    civil = new List<string>();
                    for (int i = 0; i < cvl.Length(); i++)
                    {
                        JSONObject c = cvl.GetJSONObject(i);
                        civil.Add(c.GetString("Types"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, civil);
                    sprCivilStatus.Adapter = adapter;
                }
            }
            
            
            //// Gender
            ///

            using (var client = new HttpClient())
            {
                var uri = hosting.getGender();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray gdr = jsonresult.GetJSONArray("gender");

                    gender = new List<string>();
                    for (int i = 0; i < gdr.Length(); i++)
                    {
                        JSONObject c = gdr.GetJSONObject(i);
                        gender.Add(c.GetString("Identities"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, gender);
                    sprGender.Adapter = adapter;
                }
            }

            //// Purok
            ///

            using (var client = new HttpClient())
            {
                var uri = hosting.getPurok();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray prk = jsonresult.GetJSONArray("purok");

                    purok = new List<string>();
                    for (int i = 0; i < prk.Length(); i++)
                    {
                        JSONObject c = prk.GetJSONObject(i);
                        purok.Add(c.GetString("Name"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, purok);
                    sprPurok.Adapter = adapter;
                }
            }

            //// Voter Status
            ///

            opt = new List<String>();
            opt.Add("Yes");
            opt.Add("No");
            var adapters = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, opt);
            sprVoterStatus.Adapter = adapters;
        }

        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
            alertDiag.SetTitle("Exit");
            alertDiag.SetMessage("Are you sure you want to Exit?");
            alertDiag.SetPositiveButton("OK", (senderAlert, args) => {

                base.OnBackPressed();
                Finish();
            });

            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {


            });

            Dialog diag = alertDiag.Create();
            diag.Show();
        }

    }

}