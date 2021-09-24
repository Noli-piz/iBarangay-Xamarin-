using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace iBarangayApp
{
    [Activity(Label = "Service_Module")]
    public class Service_Module : Activity
    {
        private Button BTNAdd, BTNMin, BTNRequest;
        private TextView TVQuantity, TVAvailable, tvBack;
        private Spinner sprDelivery, sprItem;
        private EditText ETPurpose;

        private List<string> DOption = new List<string>(), Item= new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_service_module);

            BTNAdd = FindViewById<Button>(Resource.Id.btnAdd);
            BTNMin = FindViewById<Button>(Resource.Id.btnMin);
            BTNRequest = FindViewById<Button>(Resource.Id.btnRequest);
            ETPurpose = FindViewById<EditText>(Resource.Id.ETPurposeItem);
            TVQuantity = FindViewById<TextView>(Resource.Id.tvQuantity);
            TVAvailable = FindViewById<TextView>(Resource.Id.tvAvailable);
            sprItem = FindViewById<Spinner>(Resource.Id.spnrItem);
            sprDelivery = FindViewById<Spinner>(Resource.Id.spnerDelivery);

            LoadSpnr();

        }

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
                    sprDelivery.Adapter = adapter;
                }
            }

            ///// Items

            using (var client = new HttpClient())
            {
                var uri = hosting.getBarangayitems();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray cvl = jsonresult.GetJSONArray("items");

                    Item = new List<string>();
                    for (int i = 0; i < cvl.Length(); i++)
                    {
                        JSONObject c = cvl.GetJSONObject(i);
                        Item.Add(c.GetString("ItemName"));
                    }

                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Item);
                    sprItem.Adapter = adapter;
                }
            }

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

                Intent intent = new Intent(this, typeof(MainRequest));
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