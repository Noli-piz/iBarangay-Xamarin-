using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iBarangayApp
{
    [Activity(Label = "Request_Module")]
    public class Request_Module : Activity
    {
        private Spinner sprDO, sprCert;
        private Button btnSubmit;
        private EditText edtPurpose;
        private TextView tvBack, tvDocFee, tvDeliveryFee;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_request_module);

            sprDO = FindViewById<Spinner>(Resource.Id.spnrDeliveryOption);
            sprCert = FindViewById<Spinner>(Resource.Id.spnrCertificate);
            btnSubmit = FindViewById<Button>(Resource.Id.btnRequest);
            edtPurpose = FindViewById<EditText>(Resource.Id.ETPurposeCert);
            tvBack = FindViewById<TextView>(Resource.Id.tvBack);
            tvDocFee = FindViewById<TextView>(Resource.Id.tvDocFee);
            tvDeliveryFee = FindViewById<TextView>(Resource.Id.tvDeliveryFee);

            LoadSpnr();

            sprDO.ItemSelected += spDO_Click;
            sprCert.ItemSelected += spC_Click;

            btnSubmit.Click += btnSubmit_Click;
            tvBack.Click += tvBack_Click;
        }

        private String strDO, strCert;
        private void spDO_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strDO = DOption[e.Position].ToString();
        }

        private void spC_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strCert = Cert[e.Position].ToString();
            tvDocFee.Text = "Document Fee: " + DocFee[e.Position].ToString();
            tvDeliveryFee.Text = "Delivery Fee: " + DeliveryFee[e.Position].ToString();
        }

        private void btnSubmit_Click(Object sender, EventArgs e)
        {
            if (strDO == "" || strCert == "")
            {

            }
            else
            {
                InsertRequest(sender);
            }
        }

        private async void InsertRequest(Object sender)
        {
            View view = (View)sender;
            try
            {
                zsg_nameandimage name = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                DateTime dateToday = DateTime.Now;

                var uri = hosting.getInsertrequest();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = name.getStrusername();
                    datas["Certificate"] = strCert;
                    datas["Purpose"] = edtPurpose.Text;
                    datas["DateOfRequest"] = dateToday.ToString("yyyy-MM-dd");
                    datas["Status"] = "Pending";
                    datas["deliveryoption"] = strDO;

                    var response = wb.UploadValues(uri, "POST", datas);
                    responseFromServer = Encoding.UTF8.GetString(response);
                }

                if (responseFromServer == "Operation Success")
                {

                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetCancelable(false);
                    alertDiag.SetTitle("Request Success.");
                    alertDiag.SetMessage("Wait for the Barangay Official to Approve your Request.");
                    alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
                    {

                        Intent intent = new Intent(this, typeof(MainRequest));
                        StartActivity(intent);
                        Finish();
                    });

                    Dialog diag = alertDiag.Create();
                    diag.Show();

                }
                else
                {
                    Snackbar.Make(view, responseFromServer, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }

            }
            catch (Exception ex)
            {
                Snackbar.Make(view, ex.Message, Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

            }
        }


        private List<string> DOption = new List<string>(), Cert = new List<string>(), DocFee = new List<string>(), DeliveryFee = new List<string>();
        private async void LoadSpnr()
        {

            ///// Delivery Option

            zsg_hosting hosting = new zsg_hosting();
            using (var client = new HttpClient())
            {
                var uri = hosting.getDeliveryoption();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray cvl = jsonresult.GetJSONArray("deliveryoptions");

                    DOption = new List<string>();
                    for (int i = 0; i < cvl.Length(); i++)
                    {
                        JSONObject c = cvl.GetJSONObject(i);
                        DOption.Add(c.GetString("Options"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, DOption);
                    sprDO.Adapter = adapter;
                }
            }

            ///// Certificate

            using (var client = new HttpClient())
            {
                var uri = hosting.getCertificate();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray crt = jsonresult.GetJSONArray("certificate");

                    Cert = new List<string>();
                    DocFee = new List<string>();
                    DeliveryFee = new List<string>();

                    for (int i = 0; i < crt.Length(); i++)
                    {
                        JSONObject c = crt.GetJSONObject(i);
                        Cert.Add(c.GetString("Types"));
                        DocFee.Add(c.GetString("DocFee"));
                        DeliveryFee.Add(c.GetString("DeliveryFee"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Cert);
                    sprCert.Adapter = adapter;
                }
            }

        }

        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            StartActivity(new Intent(this, typeof(MainRequest)).SetFlags(ActivityFlags.ReorderToFront));
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }
    }
}