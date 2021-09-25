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
    [Activity(Label = "Ser_Fragment")]
    public class Ser_Fragment : Activity
    {
        private TextView tvBack, tvDocument, tvDate, tvPurpose, tvDO, tvStatus, tvQuantity;

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

            tvDocument.Text = "Item: " + Intent.GetStringExtra("Item");
            tvDate.Text = "Requested Date: " + Intent.GetStringExtra("Date");
            tvQuantity.Text = "Quantity: " + Intent.GetStringExtra("Quantity");
            tvPurpose.Text = "Purpose: " + Intent.GetStringExtra("Purpose");
            tvStatus.Text = "Status: " + Intent.GetStringExtra("Status");
            tvDO.Text = "Delivery Option: " + Intent.GetStringExtra("DO");

            tvBack.Click += tvBack_Click;
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

                Intent intent = new Intent(this, typeof(MainService));
                StartActivity(intent);
                Finish();
            });

            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {


            });

            Dialog diag = alertDiag.Create();
            diag.Show();
        }
    }
}