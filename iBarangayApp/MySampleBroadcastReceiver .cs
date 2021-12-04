using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iBarangayApp
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.xamarin.example.TEST" })]
    class MySampleBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Log.Debug("BroadCast", "OnReceive");
        }
    }
}