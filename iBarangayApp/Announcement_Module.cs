using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace iBarangayApp
{
    [Activity(Label = "Announcement_Module")]
    public class Announcement_Module : Activity
    {
        private TextView tvBack, tvSub, tvDate, tvDetails;
        private ImageView imgView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_announcement_module);

            // Create your application here
            tvBack = FindViewById<TextView>(Resource.Id.tv_Back);
            tvSub = FindViewById<TextView>(Resource.Id.tvSubject);
            tvDate = FindViewById<TextView>(Resource.Id.tvDate);
            tvDetails = FindViewById<TextView>(Resource.Id.tvDetails);
            imgView = FindViewById<ImageView>(Resource.Id.imageView);

            tvBack.Click += Back_Click;

            tvSub.Text = Intent.GetStringExtra("Subject");
            tvDate.Text = Intent.GetStringExtra("Date");
            tvDetails.Text = Intent.GetStringExtra("Detail");
            DownloadImage(Intent.GetStringExtra("ImgLoc"));
        }

        public async void DownloadImage(String url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            imgView.SetImageBitmap(imageBitmap);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MainAnnouncement)).SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }
    }
}