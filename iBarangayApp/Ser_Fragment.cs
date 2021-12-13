using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace iBarangayApp
{
    [Activity(Label = "Ser_Fragment")]
    public class Ser_Fragment : Activity
    {
        private TextView tvBack, tvDocument, tvDate, tvPurpose, tvDO, 
            tvStatus, tvQuantity, tvNote, tvRental, tvDeadline, lblDeadline, lblNote;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.service_view);


            // Create your application here
            tvBack = FindViewById<TextView>(Resource.Id.ser_tvback);
            tvDocument = FindViewById<TextView>(Resource.Id.ser_doctype);
            tvDate = FindViewById<TextView>(Resource.Id.ser_reqdate);
            tvQuantity = FindViewById<TextView>(Resource.Id.ser_quantity);
            tvPurpose = FindViewById<TextView>(Resource.Id.ser_purpose);
            tvStatus = FindViewById<TextView>(Resource.Id.ser_status);
            tvDO = FindViewById<TextView>(Resource.Id.ser_do);
            tvNote = FindViewById<TextView>(Resource.Id.ser_note);
            tvRental = FindViewById<TextView>(Resource.Id.ser_rentaldate);
            tvDeadline = FindViewById<TextView>(Resource.Id.ser_deadline);
            lblDeadline = FindViewById<TextView>(Resource.Id.lbl_deadline);
            lblNote = FindViewById<TextView>(Resource.Id.lbl_note);


            tvDocument.Text = Intent.GetStringExtra("Item");
            tvDate.Text = Intent.GetStringExtra("Date");
            tvQuantity.Text =  Intent.GetStringExtra("Quantity");
            tvPurpose.Text =  Intent.GetStringExtra("Purpose");
            tvStatus.Text =  Intent.GetStringExtra("Status");
            tvDO.Text =   Intent.GetStringExtra("DO");
            tvRental.Text =  Intent.GetStringExtra("Rental");
            tvNote.Text =  Intent.GetStringExtra("Note");
            tvDeadline.Text =  Intent.GetStringExtra("Deadline");

            if (tvNote.Text == "" || tvNote.Text ==null || tvNote.Text =="null")
            {
                tvNote.Visibility = Android.Views.ViewStates.Gone;
                lblNote.Visibility = Android.Views.ViewStates.Gone;
            }
            if (tvDeadline.Text=="0000-00-00")
            {
                tvDeadline.Visibility = Android.Views.ViewStates.Gone;
                lblDeadline.Visibility = Android.Views.ViewStates.Gone;
            }
            tvBack.Click += tvBack_Click;
        }


        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {

            Intent intent = new Intent(this, typeof(MainService));
            StartActivity(intent);
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

        }
    }
}