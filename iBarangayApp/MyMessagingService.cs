using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using System;
using System.Collections.Generic;

namespace iBarangayApp
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyMessagingService : FirebaseMessagingService
    {

        private readonly string NOTIFICATION_CHANNEL_ID = "balangkas.valenzuela.ibarangayapp";
        //public override void OnNewToken(string token)
        //{
        //   super.onNewToken(token);
        //   Log.d("NEW_TOKEN",token);
        //}

        public override void OnMessageReceived(RemoteMessage message)
        {
            if (!message.Data.GetEnumerator().MoveNext())
            {
                SendNotification(message.GetNotification().Title, message.GetNotification().Body);
            }
            else
            {
                SendNotification(message.Data);
            }

            Log.Debug("BroadCast", "OnMessageReceive");

            Intent broad = new Intent("RefreshData");
            Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).SendBroadcast(broad);
        }

        private void SendNotification(IDictionary<string, string> data)
        {
            string title, body;
            data.TryGetValue("title", out title);
            data.TryGetValue("body", out body);

            SendNotification(title, body);
        }

        public void SendNotification(string title, string body)
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                zsg_nameandimage nme = new zsg_nameandimage();

                //To notify all user
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "Notification Channel",
                    Android.App.NotificationImportance.High);

                notificationChannel.Description = "ibarangay";
                notificationChannel.Description = nme.getStrusername();
                notificationChannel.EnableLights(true);
                notificationChannel.LightColor = Color.Red;
                notificationChannel.SetVibrationPattern(new long[] { 0, 1000, 500, 1000 });

                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            //Announcement
            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notificationBuilder.SetAutoCancel(true)
                .SetDefaults(-1)
                .SetWhen(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                .SetContentTitle(title)
                .SetContentText(body)
                .SetSmallIcon(Resource.Drawable.outline_notifications_24)
                .SetContentInfo("info");

            notificationManager.Notify(new Random().Next(), notificationBuilder.Build());
        }
    }
}