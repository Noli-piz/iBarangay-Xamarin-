using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;

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