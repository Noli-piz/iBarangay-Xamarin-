using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using Microsoft.WindowsAzure.Storage;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Android.App.DatePickerDialog;

namespace iBarangayApp
{
    [Activity(Label = "UpdateInfo_Module")]
    public class UpdateInfo_Module : Activity, IOnDateSetListener
    {
        private Button btnRegistration, btnBirthDate, btnUpdate;
        private ImageView imgProfile;
        private Spinner sprCivilStatus, sprGender, sprPurok, sprVoterStatus;
        private EditText ETFname, ETMname, ETLname, ETSname, ETBirthPlace, ETCedulaNo, ETContactNo, ETHouseAndStreet;
        private TextView tvBack;
        private ProgressBar pb;

        public static readonly int PickImageId = 1000;

        private const int BDATE_DIALOG = 1, RDATE_DIALOG = 2;
        private int Byear, Bmonth, Bday, Ryear, Rmonth, Rday;

        private string strEmail, strPassword, strImageUrl;

        private zsg_nameandimage nme = new zsg_nameandimage();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UpdateInfo_Module);

            imgProfile = FindViewById<ImageView>(Resource.Id.img_Profile3);
            btnRegistration = FindViewById<Button>(Resource.Id.btnDateOfRegistration);
            btnBirthDate = FindViewById<Button>(Resource.Id.btnBirthDate);
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);

            ETFname = FindViewById<EditText>(Resource.Id.S2Fname);
            ETMname = FindViewById<EditText>(Resource.Id.S2Mname);
            ETLname = FindViewById<EditText>(Resource.Id.S2Lname);
            ETSname = FindViewById<EditText>(Resource.Id.S2Sname);
            ETBirthPlace = FindViewById<EditText>(Resource.Id.S2Birthplace);
            ETCedulaNo = FindViewById<EditText>(Resource.Id.ETCedulaNo);
            ETContactNo = FindViewById<EditText>(Resource.Id.ETContactNo);
            ETHouseAndStreet = FindViewById<EditText>(Resource.Id.HouseAndStreet);

            sprCivilStatus = FindViewById<Spinner>(Resource.Id.spnrCivilStatus);
            sprGender = FindViewById<Spinner>(Resource.Id.spnrGender);
            sprPurok = FindViewById<Spinner>(Resource.Id.spnrPurok);
            sprVoterStatus = FindViewById<Spinner>(Resource.Id.spnrVoterStatus);

            tvBack = FindViewById<TextView>(Resource.Id.lblBack2);
            pb = FindViewById<ProgressBar>(Resource.Id.pb);

            LoadSpinners();

            imgProfile.Click += ImageClick;

            btnBirthDate.Click += delegate
            {
                ShowDialog(BDATE_DIALOG);
            };

            btnRegistration.Click += delegate
            {
                ShowDialog(RDATE_DIALOG);
            };

            btnUpdate.Click += btnUpdate_Click;

            sprCivilStatus.ItemSelected += spCV_Click;
            sprGender.ItemSelected += spG_Click;
            sprPurok.ItemSelected += spPRK_Click;
            sprVoterStatus.ItemSelected += spVS_Click;

            tvBack.Click += tvBack_Click;

            // Load Information
            imgProfile.SetImageBitmap(nme.getImg());
            ETFname.Text = nme.getFirstName();
            ETMname.Text = nme.getMiddleName();
            ETLname.Text = nme.getLastName();
            ETSname.Text = nme.getSuffixName();

            ETBirthPlace.Text = nme.getBirthPlace();
            ETCedulaNo.Text = nme.getCedulaNo();
            ETContactNo.Text = nme.getContactNo();
            ETHouseAndStreet.Text = nme.getHouseNoAndStreet();

            strImageUrl = nme.getStrImg();
            SetDate();

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
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ETFname.Text == "" || ETLname.Text == "" || ETBirthPlace.Text == ""  || ETContactNo.Text == "")
            {
                Toast.MakeText(this, "Please Insert Valid Information.", ToastLength.Short).Show();
            }
            else if (strCV == "" || strGDR == "" || strPRK == "" || strVS == "")
            {
                Toast.MakeText(this, "Please Select Valid Information.", ToastLength.Short).Show();
            }
            else
            {
                InsertInformation(sender);
            }
        }

        private async void InsertInformation(object sender)
        {

            String[] field = new String[16];
            field[0] = "Username";
            field[1] = "Fname";
            field[2] = "Mname";
            field[3] = "Lname";
            field[4] = "Sname";
            field[5] = "Birthplace";
            field[6] = "Birthdate";
            field[7] = "CivilStatus";
            field[8] = "Gender";
            field[9] = "id_purok";
            field[10] = "VoterStatus";
            field[11] = "DateOfRegistration";
            field[12] = "ContactNo";
            field[13] = "CedulaNo";
            field[14] = "Image";
            field[15] = "HouseNoAndStreet";

            String[] data = new String[16];
            data[0] = nme.getStrusername();
            data[1] = ETFname.Text;
            data[2] = ETMname.Text;
            data[3] = ETLname.Text;
            data[4] = ETSname.Text == "" ? "NONE" : ETSname.Text;
            data[5] = ETBirthPlace.Text;
            data[6] = Bdate;
            data[7] = strCV;
            data[8] = strGDR;
            data[9] = strPRK;
            data[10] = strVS;
            data[11] = Rdate;
            data[12] = ETContactNo.Text;
            data[13] = ETCedulaNo.Text;
            data[14] = strImageUrl;
            data[15] = ETHouseAndStreet.Text;


            zsg_hosting hosting = new zsg_hosting();
            var uri = hosting.getUpdatePersonalinfo();

            try
            {
                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    for (int i = 0; i < field.Length; i++)
                    {
                        datas[field[i].ToString()] = data[i].ToString();
                    }

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Update Failed")
                {

                    View view = (View)sender;
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
                else if (responseFromServer == "Update Success")
                {
                    nme.nameandimage();

                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("Update Information.");
                    alertDiag.SetMessage("You Successfuly Update your Information.");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {

                        Intent intent = new Intent(this, typeof(MainAnnouncement));
                        StartActivity(intent);
                        Finish();
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();

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
                try
                {
                    Android.Net.Uri filePath = data.Data;


                    if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.P)
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
                btnUpdate.Enabled = false;

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
                btnUpdate.Enabled = true;

            }
        }


        /// <summary>
        /// Date
        /// </summary>
        private void SetDate()
        {
            Byear = nme.getBYear();
            Bmonth = nme.getBMonth();
            Bday = nme.getBDay();

            Ryear = nme.getRYear();
            Rmonth = nme.getRMonth();
            Rday = nme.getRDay();

            DateTime Bdatetime = new DateTime(Byear, Bmonth, Bday);
            DateTime Rdatetime = new DateTime(Ryear, Rmonth, Rday);

            Bdate = Bdatetime.ToString("yyyy-MM-dd");
            Rdate = Rdatetime.ToString("yyyy-MM-dd");

            btnBirthDate.Text = Bdatetime.ToString("MMM dd, yyyy");
            btnRegistration.Text = Rdatetime.ToString("MMM dd, yyyy");

        }

        int SelectedNum = 0;
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case BDATE_DIALOG:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, Byear, Bmonth - 1, Bday);
                    }
                case RDATE_DIALOG:
                    {
                        SelectedNum = id;
                        return new DatePickerDialog(this, this, Ryear, Rmonth - 1, Rday);
                    }
                default:
                    break;
            }

            return base.OnCreateDialog(id);
        }

        private String Bdate, Rdate;
        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {

            if (SelectedNum == 2)
            {
                this.Ryear = year;
                this.Rmonth = month + 1;
                Rday = dayOfMonth;

                DateTime date = new DateTime(this.Ryear, this.Rmonth, Rday);
                btnRegistration.Text = date.ToString("MMMM dd, yyyy");
                Rdate = date.ToString("yyyy-MM-dd");
            }
            else
            {
                this.Byear = year;
                this.Bmonth = month + 1;
                Bday = dayOfMonth;

                DateTime date1 = new DateTime(this.Byear, this.Bmonth, Bday);
                btnBirthDate.Text = date1.ToString("MMMM dd, yyyy");
                Bdate = date1.ToString("yyyy-MM-dd");
            }
        }


        private List<string> civil = new List<string>(), gender = new List<string>(), purok = new List<string>(), opt = new List<string>();
        private async void LoadSpinners()
        {

            try
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
                        sprCivilStatus.SetSelection(adapter.GetPosition(nme.getCiviStatus()));
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
                        sprGender.SetSelection(adapter.GetPosition(nme.getGender()));
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
            catch (Exception ex)
            {
                Toast.MakeText(this, "Something went wrong.", ToastLength.Short).Show();
            }
        }

        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MainAnnouncement));
            StartActivity(intent);
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

        }
    }
}