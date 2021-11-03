using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Iid;
using Android.Util;

namespace iBarangayApp
{

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIDInstance : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            SendTokenToServer(refreshedToken);
        }

        private void SendTokenToServer(string refreshedToken)
        {
            Log.Debug(PackageName, refreshedToken);
        }
    }
}