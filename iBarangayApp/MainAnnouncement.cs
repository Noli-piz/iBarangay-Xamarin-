﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Newtonsoft.Json;
using Org.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using Android.Widget;
using Java.Util;
using AndroidX.SwipeRefreshLayout.Widget;
using Android.Graphics;
using System.Net;
using Android.Graphics.Drawables;

namespace iBarangayApp
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [Activity(Label = "iBarangay", NoHistory = true)]
    public class MainAnnouncement : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        private NavigationView navigationView;
        private RelativeLayout rout;
        private SwipeRefreshLayout swipe;
        private ListView lview;
        private TextView TvName;
        private ImageView imgView;

        private List<Announcement> announcementArrayList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            rout = FindViewById<RelativeLayout>(Resource.Id.Rlayout);
            lview = FindViewById<ListView>(Resource.Id.listview);


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_announcement);

            zsg_nameandimage nme = new zsg_nameandimage();
            View view = navigationView.GetHeaderView(0);
            TvName = view.FindViewById<TextView>(Resource.Id.tvMenuName);
            TvName.Text = nme.getStrname();
            imgView = view.FindViewById<ImageView>(Resource.Id.imgMenuProfile);
            if(nme.getImg() != null)
            {
                imgView.SetImageBitmap(nme.getImg());
            }

            swipe = FindViewById<SwipeRefreshLayout>(Resource.Id.refreshContent); 
            swipe.SetColorSchemeColors(Color.Red, Color.Yellow, Color.Blue);
            swipe.Refreshing = true;
            swipe.Refresh += RefreshLayout;


            GetAnnouncement();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_announcement)
            {
                
            }
            else if (id == Resource.Id.nav_request)
            {
                Intent intent = new Intent(this, typeof(MainRequest));
                intent.AddFlags(ActivityFlags.NoAnimation);
                this.Window.TransitionBackgroundFadeDuration = 0;
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_service)
            {
                Intent intent = new Intent(this, typeof(MainService));
                intent.AddFlags(ActivityFlags.NoAnimation);
                this.Window.TransitionBackgroundFadeDuration = 0;
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_logout)
            {
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("Confirm Logout");
                alertDiag.SetMessage("Are you sure you want to logout?");
                alertDiag.SetPositiveButton("Logout", (senderAlert, args) => {

                    Intent intent = new Intent(this, typeof(Login));
                    StartActivity(intent);
                    Finish();
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
                
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        List<string> LId = new List<string>(), LDate = new List<string>(), LSubject = new List<string>(), LLevel = new List<string>(), LImageLocation = new List<string>(), LDetail = new List<string>();
        private async void GetAnnouncement()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    zsg_hosting hosting = new zsg_hosting();
                    //var uri = "http://192.168.254.114/iBarangay/ibarangay_announcement.php";
                    var uri = hosting.getAnnouncement();
                    var result = await client.GetStringAsync(uri);


                    JSONObject jsonresult = new JSONObject(result);
                    int success = jsonresult.GetInt("success");

                    if (success == 1)
                    {
                        JSONArray ann = jsonresult.GetJSONArray("announcement");


                        for (int i = 0; i < ann.Length(); i++)
                        {
                            JSONObject ancmnt = ann.GetJSONObject(i);

                            LId.Add(ancmnt.GetInt("id_announcement").ToString());
                            LDate.Add(ancmnt.GetString("Date"));
                            LSubject.Add(ancmnt.GetString("Subject"));
                            LLevel.Add(ancmnt.GetString("Level"));
                            LImageLocation.Add(ancmnt.GetString("ImageLocation"));
                            LDetail.Add(ancmnt.GetString("Details"));
                        }

                        announcementArrayList = new List<Announcement>();
                        for (int i = 0; i < ann.Length(); i++)
                        {
                            Announcement announcem = new Announcement()
                            {
                                id_announcement = Int32.Parse(LId[i].ToString()),
                                Date = LDate[i].ToString(),
                                Subject = LSubject[i].ToString(),
                                Level = LLevel[i].ToString(),
                                ImageLocation = LImageLocation[i].ToString(),
                                Details = LDetail[i].ToString()
                            };

                            announcementArrayList.Add(announcem);
                        }

                        var adapter = new CustomAdapter(this, announcementArrayList);
                        lview.Adapter = adapter;
                        lview.ItemClick += List_Click;
                    }
                    else
                    {
                        Snackbar.Make(rout, "Failed to Load", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Make(rout, "Unable to connect to the database.", Snackbar.LengthLong).SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }
            finally{
                swipe.Refreshing = false;
            }
        }


        public void List_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Announcement_Module));
            intent.PutExtra("Date", LDate[e.Position].ToString());
            intent.PutExtra("Subject", LSubject[e.Position]);
            intent.PutExtra("Level", LLevel[e.Position]);
            intent.PutExtra("ImgLoc", LImageLocation[e.Position]);
            intent.PutExtra("Detail", LDetail[e.Position]);
            StartActivity(intent);
        }
        private void RefreshLayout(object sender, EventArgs e)
        {
            GetAnnouncement();
        }


        private Bitmap DownloadImage(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}

