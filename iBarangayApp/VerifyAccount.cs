using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iBarangayApp
{
    [Activity(Label = "VerifyAccount")]
    public class VerifyAccount : Activity
    {

        private Button btnSubmit, btnPickImg, btnOpenCam;
        private ImageView imgView;
        private ProgressBar pb;
        private TextView tvCancel;

        private MemoryStream inputStream;
        private string strImageUrl;
        private static Bitmap mBitMap;
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VerifyAccount);
            // Create your application here

            imgView = FindViewById<ImageView>(Resource.Id.imgVerify);
            pb = FindViewById<ProgressBar>(Resource.Id.pb);
            tvCancel = FindViewById<TextView>(Resource.Id.lblCancel);

            btnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
            btnOpenCam = FindViewById<Button>(Resource.Id.btnOpenCam);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);


            btnPickImg.Click += btnPickImg_Click;
            btnOpenCam.Click += btnOpenCam_Click;
            btnSubmit.Click += btnSubmit_Click;
            tvCancel.Click += delegate
            {
                OnBackPressed();
            };

            if (mBitMap != null)
            {
                imgView.SetImageBitmap(mBitMap);
            }
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

        private void btnSubmit_Click(Object sender, EventArgs e)
        {
            //this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.NotTouchable);
            //this.Window.ClearFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.NotTouchable);
            if (strImageUrl != null || strImageUrl == "")
            {
                Intent intent = new Intent(this, typeof(VerifyAccount2));
                intent.PutExtra("ID", strImageUrl);
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Please Select an Image.", ToastLength.Short);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            try { 
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
                    MemoryStream inputStream = new MemoryStream(bitmapData);
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
                pb.Visibility = ViewStates.Visible;
                btnSubmit.Enabled = false;

                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ibarangaystorage;AccountKey=SuJ5YP5ovCzjeBc9sLKwbbhrk8GIWjrSyO493EnTRLc7tpNxApS/sdsIvk+qXWOhohgVASKI6VjFgrCYGYiuEw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("validationimages");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(stream);
                string URL = blockBlob.Uri.OriginalString;
                strImageUrl = URL;
                Toast.MakeText(this, "Image uploaded to Blob Storage Successfully!", ToastLength.Short).Show();

            }
            catch (Exception e)
            {
                Toast.MakeText(this, "" + e.ToString(), ToastLength.Short).Show();
            }
            finally
            {
                pb.Visibility = ViewStates.Invisible;
                btnSubmit.Enabled = true;
            }
        }

        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
            alertDiag.SetTitle("Quit Verification?");
            alertDiag.SetMessage("Are you sure you want to quit?");
            alertDiag.SetPositiveButton("Okay", (senderAlert, args) => {

                Intent intent = new Intent(this, typeof(MainAnnouncement));
                StartActivity(intent);
                Finish();
                this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
            });
            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {

                alertDiag.Dispose();

            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }
    }
}