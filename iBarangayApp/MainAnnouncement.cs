using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using Firebase.Messaging;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iBarangayApp
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    [Activity(Label = "iBarangay")]
    public class MainAnnouncement : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        private NavigationView navigationView;
        private RelativeLayout rout;
        private SwipeRefreshLayout swipe;
        private ListView lview;
        private TextView TvName, TvVerificationStatus;
        private ImageView imgView;

        private static List<Announcement> announcementArrayList;
        private static CustomAdapter adapter;
        private zsg_nameandimage nme = new zsg_nameandimage();
        private ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        //Refresh the Activity
        private MyBroadcastReceiver receiver;
        public static MainAnnouncement instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MainAnnouncement.instance = this;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            rout = FindViewById<RelativeLayout>(Resource.Id.Rlayout);
            lview = FindViewById<ListView>(Resource.Id.listview);

            // Initialized Drawer Layout
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();



            //Initialized Navigation Information
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_announcement);

            View view = navigationView.GetHeaderView(0);
            TvName = view.FindViewById<TextView>(Resource.Id.tvMenuName);
            TvVerificationStatus = view.FindViewById<TextView>(Resource.Id.tvVerificationStatus);

            TvName.Text = nme.getStrname() == null ? "Loading..." : nme.getStrname();
            TvVerificationStatus.Text = nme.getboolVerified() == false ? "Not Verified" : "Verified";
            imgView = view.FindViewById<ImageView>(Resource.Id.imgMenuProfile);
            if (nme.getImg() != null)
            {
                imgView.SetImageBitmap(nme.getImg());
            }

            imgView.Clickable = true;
            imgView.Click += (s, e) =>
            {
                StartActivity(new Intent(this, typeof(UpdateInfo_Module)));
            };

            //  Get Announcement
            swipe = FindViewById<SwipeRefreshLayout>(Resource.Id.refreshContent);
            swipe.SetColorSchemeColors(Color.Red, Color.Yellow, Color.Blue);
            swipe.Refreshing = true;
            swipe.Refresh += RefreshLayout;

            GetAnnouncement();

            // Subscribe to FCM
            FirebaseMessaging.Instance.SubscribeToTopic("ibarangay");
            FirebaseMessaging.Instance.SubscribeToTopic(nme.getStrusername());


            //Refresh The Activity
            receiver = new MyBroadcastReceiver();
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                Intent startMain = new Intent(Intent.ActionMain);
                startMain.AddCategory(Intent.CategoryHome);
                startMain.SetFlags(ActivityFlags.NewTask);
                StartActivity(startMain);
                //base.OnBackPressed();
            }
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    if (id == Resource.Id.action_settings)
        //    {
        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            if (id == Resource.Id.nav_announcement)
            {

            }
            else if (id == Resource.Id.nav_request)
            {
                if (nme.getboolVerified() == true)
                {
                    Intent intent = new Intent(this, typeof(MainRequest));
                    intent.AddFlags(ActivityFlags.NoAnimation);
                    StartActivity(intent);
                    this.Window.TransitionBackgroundFadeDuration = 0;
                }
                else
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetCancelable(false);
                    alertDiag.SetTitle("Not Verified");
                    alertDiag.SetMessage("Unable to access because you are still not Verified.");
                    alertDiag.SetPositiveButton("Get Verified", (senderAlert, args) =>
                    {

                        StartActivity(new Intent(this, typeof(VerifyAccount)));
                        Finish();

                    });
                    alertDiag.SetNegativeButton("Do it Later", (senderAlert, args) =>
                    {

                        navigationView.SetCheckedItem(Resource.Id.nav_announcement);
                        alertDiag.Dispose();
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }
            else if (id == Resource.Id.nav_service)
            {
                if (nme.getboolVerified() == true)
                {
                    Intent intent = new Intent(this, typeof(MainService));
                    intent.AddFlags(ActivityFlags.NoAnimation);
                    this.Window.TransitionBackgroundFadeDuration = 0;
                    StartActivity(intent);
                }
                else
                {
                    Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                    alertDiag.SetCancelable(false);
                    alertDiag.SetTitle("Not Verified");
                    alertDiag.SetMessage("Unable to access because you are still not Verified.");
                    alertDiag.SetPositiveButton("Get Verified", (senderAlert, args) =>
                    {

                        StartActivity(new Intent(this, typeof(VerifyAccount)));
                        Finish();

                    });
                    alertDiag.SetNegativeButton("Do it Later", (senderAlert, args) =>
                    {

                        navigationView.SetCheckedItem(Resource.Id.nav_announcement);
                        alertDiag.Dispose();
                    });
                    Dialog diag = alertDiag.Create();
                    diag.Show();
                }
            }
            else if (id == Resource.Id.nav_logout)
            {
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetCancelable(false);
                alertDiag.SetTitle("Confirm Logout");
                alertDiag.SetMessage("Are you sure you want to logout?");
                alertDiag.SetPositiveButton("Logout", (senderAlert, args) =>
                {

                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.Clear();
                    edit.PutString("Logout", "true");
                    edit.Apply();


                    Intent intent = new Intent(this, typeof(Login)).SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                    this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
                {

                    navigationView.SetCheckedItem(Resource.Id.nav_announcement);
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

                            announcem.DownloadImage(LImageLocation[i].ToString());
                            announcementArrayList.Add(announcem);
                        }

                        adapter = new CustomAdapter(this, announcementArrayList);
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
            finally
            {
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
            LId.Clear();
            LDate.Clear();
            LSubject.Clear();
            LLevel.Clear();
            LImageLocation.Clear();
            LDetail.Clear();

            GetAnnouncement();
        }


        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("BroadCast", "Resume");

            Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).RegisterReceiver(receiver, new IntentFilter("RefreshData"));
        }

        protected override void OnPause()
        {
            Log.Debug("BroadCast", "Pause");
            Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).UnregisterReceiver(receiver);

            base.OnPause();
        }

        public async void UpdateGuiMethod()
        {
            nme.retrievedVerificationStatus();

            await Task.Delay(5000);
            //this.Recreate();
            //OverridePendingTransition(0, 0);
            TvVerificationStatus.Text = nme.getboolVerified() == false ? "Not Verified" : "Verified";

        }

        [BroadcastReceiver]
        public class MyBroadcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                MainAnnouncement.instance.UpdateGuiMethod();
            }
        }
    }
}

