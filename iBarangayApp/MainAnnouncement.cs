using System;
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

namespace iBarangayApp
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    [Activity(Label = "@string/app_name")]
    public class MainAnnouncement : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        private NavigationView navigationView;
        private RelativeLayout rout;
        private ListView lview;

        private List<Announcement> announcementArrayList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            rout = FindViewById<RelativeLayout>(Resource.Id.Rlayout);
            lview = FindViewById<ListView>(Resource.Id.listview);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_announcement);


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

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
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
            using (var client = new HttpClient())
            {
                var uri = "http://192.168.254.114/iBarangay/ibarangay_announcement.php";
                var result = await client.GetStringAsync(uri);


                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success== 1)
                {
                    JSONArray ann = jsonresult.GetJSONArray("announcement");


                    for (int i =0; i < ann.Length() ; i++) {
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
    }
}

