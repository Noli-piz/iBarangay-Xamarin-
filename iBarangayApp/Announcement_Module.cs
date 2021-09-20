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
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainAnnouncement)).SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);
            Finish();
        }
    }
}