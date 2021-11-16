﻿using Android.App;
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
    [Activity(Label = "Req_Fragment")]
    public class Req_Fragment : Activity
    {
        private TextView tvBack, tvDocument, tvDate, tvPurpose, tvDO, tvStatus, tvNote;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.request_view);

            // Create your application here
            tvBack = FindViewById<TextView>(Resource.Id.req_tvback);
            tvDocument = FindViewById<TextView>(Resource.Id.req_doctype);
            tvDate = FindViewById<TextView>(Resource.Id.req_reqdate);
            tvPurpose = FindViewById<TextView>(Resource.Id.req_purpose);
            tvStatus = FindViewById<TextView>(Resource.Id.req_status);
            tvDO = FindViewById<TextView>(Resource.Id.req_do);
            tvNote = FindViewById<TextView>(Resource.Id.req_note);

            tvDocument.Text = "Document: " + Intent.GetStringExtra("Item");
            tvDate.Text = "Requested Date: " + Intent.GetStringExtra("Date");
            tvPurpose.Text = "Purpose: " + Intent.GetStringExtra("Purpose");
            tvStatus.Text = "Status: " + Intent.GetStringExtra("Status");
            tvDO.Text = "Delivery Option: " + Intent.GetStringExtra("DO");
            tvNote.Text = "Note: " + Intent.GetStringExtra("Note");

            tvBack.Click += tvBack_Click;
        }


        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(MainRequest));
            StartActivity(intent);
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

        }
    }
}