using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Google.Android.Material.Snackbar;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace iBarangayApp
{
    public class frag_service1 : AndroidX.Fragment.App.Fragment
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


        List<String> ArrItem = new List<String>(), ArrQuantity = new List<string>(),
            ArrPurpose = new List<String>(), ArrDate = new List<String>(), ArrStatus = new List<String>(),
            ArrDO = new List<String>(), ArrNote = new List<String>(), ArrRent= new List<String>();
        private async void GetRequest()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_nameandimage user = new zsg_nameandimage();
                    zsg_hosting hosting = new zsg_hosting();

                    var uri = hosting.getServiceall() + "?Username=" + user.getStrusername();
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
                            ArrNote.Add(rqst.GetString("Note"));
                            ArrRent.Add(rqst.GetString("RentDate"));

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
                    //else
                    //{
                    //    Snackbar.Make(lout, "Failed to Load", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    //}

                    swipe.Refreshing = false;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message + "", ToastLength.Short).Show();
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
            intent.PutExtra("Note", ArrNote[e.Position]);
            intent.PutExtra("Rental", ArrRent[e.Position]);
            StartActivity(intent);
        }

        private void RefreshLayout(object sender, EventArgs e)
        {
            ArrItem.Clear();
            ArrQuantity.Clear();
            ArrDate.Clear();
            ArrPurpose.Clear();
            ArrStatus.Clear();
            ArrDO.Clear();
            ArrNote.Clear();
            ArrRent.Clear();

            GetRequest();
        }
    }
}