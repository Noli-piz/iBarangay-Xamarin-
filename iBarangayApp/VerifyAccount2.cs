using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iBarangayApp
{
    [Activity(Label = "VerifyAccount2")]
    public class VerifyAccount2 : Activity
    {

        private Button btnFinish, btnPickImg, btnOpenCam;
        private ImageView imgView;
        private ProgressBar pb;
        private TextView tvBack;

        private MemoryStream inputStream;
        private string strImage2Url = "", strImage1Url;
        private Bitmap mBitMap;
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VerifyAccount2);

            imgView = FindViewById<ImageView>(Resource.Id.imgVerify);
            pb = FindViewById<ProgressBar>(Resource.Id.pb);
            tvBack = FindViewById<TextView>(Resource.Id.lblBack);

            btnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
            btnOpenCam = FindViewById<Button>(Resource.Id.btnOpenCam);
            btnFinish = FindViewById<Button>(Resource.Id.btnFinish);


            btnPickImg.Click += btnPickImg_Click;
            btnOpenCam.Click += btnOpenCam_Click;
            btnFinish.Click += btnFinish_Click;
            tvBack.Click += delegate
            {
                OnBackPressed();
            };

            if (mBitMap != null)
            {
                imgView.SetImageBitmap(mBitMap);
            }


            strImage1Url = Intent.GetStringExtra("image1");

        }

        private void btnPickImg_Click(Object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        private void btnOpenCam_Click(Object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }

        private void btnFinish_Click(Object sender, EventArgs e)
        {
            if (strImage2Url == null || strImage2Url == "")
            {
                Toast.MakeText(this, "Please Select an Image.", ToastLength.Short).Show();
            }
            else
            {
                Intent intent = new Intent(this, typeof(VerifyAccount3));
                intent.PutExtra("image1", strImage1Url);
                intent.PutExtra("image2", strImage2Url);
                StartActivity(intent);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try
            {

                if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri filePath = data.Data;
                    if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.P)
                    {

                        mBitMap = MediaStore.Images.Media.GetBitmap(ContentResolver, filePath);
                        imgView.SetImageBitmap(mBitMap);
                        byte[] bitmapData;
                        using (var stream = new MemoryStream())
                        {
                            mBitMap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            bitmapData = stream.ToArray();
                        }
                        inputStream = new MemoryStream(bitmapData);
                        Upload(inputStream);
                    }
                    else
                    {

                        var source = ImageDecoder.CreateSource(ContentResolver, filePath);
                        mBitMap = ImageDecoder.DecodeBitmap(source);
                        imgView.SetImageBitmap(mBitMap);

                        byte[] bitmapData;
                        using (var stream = new MemoryStream())
                        {
                            mBitMap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            bitmapData = stream.ToArray();
                        }
                        inputStream = new MemoryStream(bitmapData);
                        Upload(inputStream);

                    }

                }
                else if (requestCode == 0 && resultCode == Result.Ok && data != null)
                {
                    Bitmap mBitMap = (Bitmap)data.Extras.Get("data");
                    imgView.SetImageBitmap(mBitMap);
                    byte[] bitmapData;
                    using (var stream = new MemoryStream())
                    {
                        mBitMap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                        bitmapData = stream.ToArray();
                    }
                    inputStream = new MemoryStream(bitmapData);
                    Upload(inputStream);

                }
            }
            catch (IOException e)
            {
                Toast.MakeText(this, "" + e.ToString(), ToastLength.Short);
            }
        }

        /// upload Blob function
        private async void Upload(Stream stream)
        {
            try
            {
                this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.NotTouchable);
                pb.Visibility = ViewStates.Visible;

                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("validationimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImage2Url = URL;
                Toast.MakeText(this, "Image uploaded to Blob Storage Successfully!", ToastLength.Long).Show();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "" + e.ToString(), ToastLength.Short).Show();
            }
            finally
            {
                pb.Visibility = ViewStates.Invisible;
                this.Window.ClearFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.NotTouchable);
            }
        }

        private async void Verification()
        {
            try
            {
                zsg_nameandimage nme = new zsg_nameandimage();
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

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Operation Failed")
                {
                    Toast.MakeText(this, "Something went wrong. Please try again later.", ToastLength.Short).Show();
                }
                else if (responseFromServer == "Operation Success")
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetTitle("Completed.");
                    alertDiag.SetMessage("Please wait atleast 3-7 days to verify your account.");
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

        public override void OnBackPressed()
        {

            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
            alertDiag.SetTitle("Go Back?");
            alertDiag.SetMessage("Are you sure you want to go back? Your data will not be save.");
            alertDiag.SetPositiveButton("Okay", (senderAlert, args) =>
            {

                base.OnBackPressed();

            });
            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
            {

                alertDiag.Dispose();

            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }
    }
}