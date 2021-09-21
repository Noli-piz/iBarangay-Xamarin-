﻿using Android.App;
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
    class frag_request1 : AndroidX.Fragment.App.Fragment
    {

        private TextView tv;
        private SwipeRefreshLayout swipe;
        private ListView lview;
        private LinearLayout lout;

        private List<RFrag> requestArrayList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //binding =  inflater.Inflate(Resource.Layout.frag1_request, container, false);

            View view = inflater.Inflate(Resource.Layout.frag1_request, null);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            tv = (TextView)view.FindViewById(Resource.Id.txt);
            swipe = (SwipeRefreshLayout)view.FindViewById(Resource.Id.refreshFrag1);
            lview = (ListView)view.FindViewById(Resource.Id.req_fragment1_listview);
            lout = (LinearLayout)view.FindViewById(Resource.Id.Lout);

            GetRequest();

            swipe.SetColorSchemeColors(Color.Red, Color.Yellow, Color.Blue);
            swipe.Refreshing = true;
            swipe.Refresh += RefreshLayout;
        }


        List<String> ArrItem = new List<String>(), ArrPurpose = new List<String>(), ArrDate = new List<String>(), ArrStatus = new List<String>();
        private async void GetRequest()
        {
            using (var client = new HttpClient())
            {
                zsg_nameandimage user = new zsg_nameandimage();
                zsg_hosting hosting = new zsg_hosting();

                var uri = hosting.getRequestall() + "?Username=" + user.getStrusername();
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray req = jsonresult.GetJSONArray("request");


                    for (int i = 0; i < req.Length(); i++)
                    {
                        JSONObject rqst = req.GetJSONObject(i);

                        ArrItem.Add(rqst.GetString("Types"));
                        ArrPurpose.Add(rqst.GetString("DateOfRequest"));
                        ArrDate.Add(rqst.GetString("Purpose"));
                        ArrStatus.Add(rqst.GetString("Status"));
                        //String strdeliveryoptions = rqst.GetString("Options");
                    }

                    requestArrayList = new List<RFrag>();
                    for (int i = 0; i < req.Length(); i++)
                    {
                        RFrag rquest = new RFrag(i, ArrItem[i].ToString(), ArrDate[i].ToString(), ArrPurpose[i].ToString(), ArrStatus[i].ToString());
                        requestArrayList.Add(rquest);
                    }

                    var adapter = new CustomAdapterRequest(this.Activity, requestArrayList);
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
            Intent intent = new Intent(this.Activity, typeof(Req_Fragment));
            intent.PutExtra("Date", ArrItem[e.Position].ToString());
            intent.PutExtra("Subject", ArrPurpose[e.Position]);
            intent.PutExtra("Level", ArrDate[e.Position]);
            intent.PutExtra("ImgLoc", ArrStatus[e.Position]);
            StartActivity(intent);
        }


        private void RefreshLayout(object sender, EventArgs e) {
            GetRequest();
        }
    }
}