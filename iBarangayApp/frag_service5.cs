using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using AndroidX.SwipeRefreshLayout.Widget;
using Google.Android.Material.Snackbar;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace iBarangayApp
{
    public class frag_service5 : AndroidX.Fragment.App.Fragment
    {
        private SwipeRefreshLayout swipe;
        private ListView lview;
        private LinearLayout lout;

        private List<SFrag> serviceArrayList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.frag1_request, null);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            swipe = (SwipeRefreshLayout)view.FindViewById(Resource.Id.refreshFrag1);
            lview = (ListView)view.FindViewById(Resource.Id.req_fragment1_listview);
            lout = (LinearLayout)view.FindViewById(Resource.Id.Lout);

            GetRequest();

            swipe.SetColorSchemeColors(Color.Red, Color.Yellow, Color.Blue);
            swipe.Refreshing = true;
            swipe.Refresh += RefreshLayout;
        }


        List<String> ArrItem = new List<String>(), ArrQuantity = new List<string>(), ArrPurpose = new List<String>(), ArrDate = new List<String>(), ArrStatus = new List<String>(), ArrDO = new List<String>();
        private async void GetRequest()
        {
            using (var client = new HttpClient())
            {
                zsg_nameandimage user = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                var uri = hosting.getServiceborrowed() + "?Username=" + user.getStrusername();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray req = jsonresult.GetJSONArray("service");


                    for (int i = 0; i < req.Length(); i++)
                    {
                        JSONObject rqst = req.GetJSONObject(i);

                        ArrItem.Add(rqst.GetString("ItemName"));
                        ArrQuantity.Add(rqst.GetString("Quantity"));
                        ArrDate.Add(rqst.GetString("DateOfRequest"));
                        ArrPurpose.Add(rqst.GetString("Purpose"));
                        ArrStatus.Add(rqst.GetString("Status"));
                        ArrDO.Add(rqst.GetString("Options"));
                    }

                    serviceArrayList = new List<SFrag>();
                    for (int i = 0; i < req.Length(); i++)
                    {
                        SFrag rquest = new SFrag(
                            i, 
                            ArrItem[i].ToString(), 
                            ArrQuantity[i].ToString(), 
                            ArrDate[i].ToString(), 
                            ArrPurpose[i].ToString(), 
                            ArrStatus[i].ToString()
                        );
                        
                        serviceArrayList.Add(rquest);
                    }

                    var adapter = new CustomAdapterService(this.Activity, serviceArrayList);
                    lview.Adapter = adapter;
                    lview.ItemClick += List_Click;
                }
                else if ("No Data" == jsonresult.GetString("message"))
                {
                    //Snackbar.Make(lout, "No Data.", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
                else
                {
                    Snackbar.Make(lout, "Failed to Load", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }

                swipe.Refreshing = false;
            }
        }

        private void List_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(Ser_Fragment));
            intent.PutExtra("Item", ArrItem[e.Position].ToString());
            intent.PutExtra("Date", ArrDate[e.Position].ToString());
            intent.PutExtra("Quantity", ArrQuantity[e.Position]);
            intent.PutExtra("Purpose", ArrPurpose[e.Position]);
            intent.PutExtra("Status", ArrStatus[e.Position]);
            intent.PutExtra("DO", ArrDO[e.Position]);
            StartActivity(intent);
        }

        private void RefreshLayout(object sender, EventArgs e)
        {
            GetRequest();
        }
    }
}