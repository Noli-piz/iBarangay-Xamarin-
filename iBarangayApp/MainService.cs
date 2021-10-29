using Android.App;
using Android.Content;
using Android.Media.TV;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionBarDrawerToggle = AndroidX.AppCompat.App.ActionBarDrawerToggle;

namespace iBarangayApp
{

    [Activity(Label = "Miscellaneous Services")]
    public class MainService : AppCompatActivity ,NavigationView.IOnNavigationItemSelectedListener
    {

        private NavigationView navigationView;
        private RelativeLayout rout;
        private ListView lview;
        private TextView TvName;

        private TabLayout tabLayout;
        private ImageView imgView;
        private ViewPager pager;
        private ISharedPreferences pref = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_service);


            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            ExtendedFloatingActionButton fab = FindViewById<ExtendedFloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            rout = FindViewById<RelativeLayout>(Resource.Id.Rlayout);
            lview = FindViewById<ListView>(Resource.Id.listview);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_service);

            //Nav Text Header
            zsg_nameandimage nme = new zsg_nameandimage();
            View view = navigationView.GetHeaderView(0);
            TvName = view.FindViewById<TextView>(Resource.Id.tvMenuName);
            TvName.Text = nme.getStrname();
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
            // Tabs

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
            pager = FindViewById<ViewPager>(Resource.Id.pager);
            PagerAdapter adapter = new PagerAdapter(SupportFragmentManager);

            adapter.AddFragment(new frag_service1(), "All");
            adapter.AddFragment(new frag_service2(), "Pending");
            adapter.AddFragment(new frag_service3(), "Approved");
            adapter.AddFragment(new frag_service4(), "Disapproved");
            adapter.AddFragment(new frag_service5(), "Barrowed");
            adapter.AddFragment(new frag_service6(), "Returned");

            pager.Adapter = adapter;
            adapter.NotifyDataSetChanged();
            tabLayout.SetupWithViewPager(pager);
            base.OnCreate(savedInstanceState);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(AndroidX.Core.View.GravityCompat.Start))
            {
                drawer.CloseDrawer(AndroidX.Core.View.GravityCompat.Start);
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

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(Service_Module));
            StartActivity(intent);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);


            if (id == Resource.Id.nav_announcement)
            {
                StartActivity(new Intent(this, typeof(MainAnnouncement)).SetFlags(ActivityFlags.NoAnimation));
                this.Window.TransitionBackgroundFadeDuration = 0;
            }
            else if (id == Resource.Id.nav_request)
            {
                StartActivity(new Intent(this, typeof(MainRequest)).SetFlags(ActivityFlags.NoAnimation));
                this.Window.TransitionBackgroundFadeDuration = 0;
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

                    ISharedPreferencesEditor edit = pref.Edit();
                    edit.Clear();
                    edit.PutString("Logout", "true");
                    edit.Apply();

                    Intent intent = new Intent(this, typeof(Login)).SetFlags( ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                    this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {

                    navigationView.SetCheckedItem(Resource.Id.nav_service);
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(AndroidX.Core.View.GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}