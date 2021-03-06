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
using static Android.App.DatePickerDialog;

namespace iBarangayApp
{
    [Activity(Label = "Service_Module")]
    public class Service_Module : Activity, IOnDateSetListener
    {
        private Button BTNAdd, BTNMin, BTNRequest, BTNRent;
        private TextView TVAvailable, tvBack, tvDelFee;
        private Spinner sprDelivery, sprItem;
        private EditText ETPurpose, TVQuantity;

        private List<string> DOption = new List<string>(), Item = new List<string>();

        private int rentDay, rentMonth, rentYear;
        private string rentDate;
        private const int DATE_DIALOG = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_service_module);

            BTNAdd = FindViewById<Button>(Resource.Id.btnAdd);
            BTNMin = FindViewById<Button>(Resource.Id.btnMin);
            BTNRequest = FindViewById<Button>(Resource.Id.btnRequestItem);
            BTNRent = FindViewById<Button>(Resource.Id.btnRentDate);
            ETPurpose = FindViewById<EditText>(Resource.Id.ETPurposeItem);
            TVQuantity = FindViewById<EditText>(Resource.Id.tvQuantity);
            TVAvailable = FindViewById<TextView>(Resource.Id.tvAvailable);
            tvBack = FindViewById<TextView>(Resource.Id.tvBack);
            tvDelFee = FindViewById<TextView>(Resource.Id.tvDeliveryFee);
            sprItem = FindViewById<Spinner>(Resource.Id.spnrItem);
            sprDelivery = FindViewById<Spinner>(Resource.Id.spnerDelivery);

            LoadSpnr();

            sprDelivery.ItemSelected += spDO_Click;
            sprItem.ItemSelected += spI_Click;

            tvBack.Click += tvBack_Click;
            BTNRequest.Click += btnSubmit_CLick;

            BTNMin.Click += btnMin_Click;
            BTNAdd.Click += btnAdd_Click;

            TVQuantity.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {

                if (TVQuantity.Text.Length > 0)
                {
                    String Newvalue = TVQuantity.Text.ToString();
                    if (Convert.ToInt32(Newvalue) > intAvailable)
                    {
                        Toast.MakeText(this, "Not Enough Quantity.", ToastLength.Short).Show();
                        TVQuantity.Text = intAvailable.ToString();
                    }
                    else
                    {
                        intQuantity = Convert.ToInt32(Newvalue);
                    }
                }
            };

            BTNRent.Click += delegate
            {
                ShowDialog(DATE_DIALOG);
            };

            SetDate();
        }


        private String strDO, strItem;
        private int intQuantity = 0, intAvailable = 0;

        private void spDO_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strDO = DOption[e.Position].ToString();
        }

        private void spI_Click(Object sender, AdapterView.ItemSelectedEventArgs e)
        {
            strItem = Item[e.Position].ToString();
            RetrieveQuantity();
        }

        private void btnMin_Click(Object sender, EventArgs e)
        {
            if (intQuantity > 0)
            {
                intQuantity--;
                TVQuantity.Text = intQuantity.ToString();
            }
        }

        private void btnAdd_Click(Object sender, EventArgs e)
        {
            if (intQuantity < intAvailable)
            {
                intQuantity++;
                TVQuantity.Text = intQuantity.ToString();
            }
        }

        private void btnSubmit_CLick(Object sender, EventArgs e)
        {
            if (TVQuantity.Text == "0")
            {
                Toast.MakeText(this, "Not Valid Quantity", ToastLength.Short).Show();

            }
            else if (ETPurpose.Text == "")
            {
                Toast.MakeText(this, "Not Valid Purpose.", ToastLength.Short).Show();
            }
            else
            {
                InsertService(sender);
            }
        }

        private async void InsertService(Object sender)
        {
            View view = (View)sender;
            try
            {
                zsg_nameandimage name = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                DateTime dateToday = DateTime.Now;

                var uri = hosting.getInsertservice();

                string responseFromServer;
                using (var wb = new WebClient())
                {
                    var datas = new NameValueCollection();
                    datas["Username"] = name.getStrusername();
                    datas["Item"] = strItem;
                    datas["Quantity"] = intQuantity.ToString();
                    datas["Purpose"] = ETPurpose.Text;
                    datas["DateOfRequest"] = dateToday.ToString("yyyy-MM-dd");
                    datas["Status"] = "Pending";
                    datas["deliveryoption"] = strDO;
                    datas["RentDate"] = rentDate;

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

                        Intent intent = new Intent(this, typeof(MainService));
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

        private async void LoadSpnr()
        {

            ///// Items
            zsg_hosting hosting = new zsg_hosting();

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

            ///// Delivery Option

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


        }


        private async void RetrieveQuantity()
        {
            try
            {

                zsg_hosting hosting = new zsg_hosting();

                using (var client = new HttpClient())
                {
                    var uri = hosting.getItemquantity() + "?ItemName=" + strItem;
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray info = jsonresult.GetJSONArray("quantity");
                        JSONObject strAvai = info.GetJSONObject(0);

                        tvDelFee.Text = "Delivery Fee: " + strAvai.GetString("DeliveryFee");

                        intAvailable = Int32.Parse(strAvai.GetString("Quantity"));
                        TVAvailable.Text = intAvailable.ToString();

                        if (intAvailable < intQuantity)
                        {
                            TVQuantity.Text = intAvailable.ToString();
                            intQuantity = intAvailable;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }


        private void SetDate()
        {
            rentYear = Int32.Parse(DateTime.Now.ToString("yyyy"));
            rentMonth = Int32.Parse(DateTime.Now.ToString("MM"));
            rentDay = Int32.Parse(DateTime.Now.ToString("dd"));

            DateTime date = new DateTime(rentYear, rentMonth, rentDay);

            rentDate = date.ToString("yyyy-MM-dd");
            BTNRent.Text = date.ToString("MMM dd, yyyy");

        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        return new DatePickerDialog(this, this, rentYear, rentMonth - 1, rentDay);
                    }
                default:
                    break;
            }

            return base.OnCreateDialog(id);
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            rentYear = year;
            rentMonth = month + 1;
            rentDay = dayOfMonth;

            DateTime date = new DateTime(rentYear, rentMonth, rentDay);
            BTNRent.Text = date.ToString("MMMM dd, yyyy");
            rentDate = date.ToString("yyyy-MM-dd");
        }


        private void tvBack_Click(Object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            StartActivity(new Intent(this, typeof(MainService)).SetFlags(ActivityFlags.ReorderToFront));
            Finish();
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
        }
    }
}