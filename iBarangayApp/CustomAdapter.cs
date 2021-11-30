using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace iBarangayApp
{

    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Announcement> announcement;

        public CustomAdapter(Activity activity, List<Announcement> announcement)
        {

            this.activity = activity;
            this.announcement = announcement;

        }
        public override int Count
        {
            get
            {
                return announcement.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return announcement[position].id_announcement;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.announcement_list_item, parent, false);

            var txtSubject = view.FindViewById<TextView>(Resource.Id.subject);
            var txtDetails = view.FindViewById<TextView>(Resource.Id.details);
            var txtDate = view.FindViewById<TextView>(Resource.Id.date);
            var imgProfile = view.FindViewById<ImageView>(Resource.Id.alert_pic);

            txtSubject.Text = "Subject: " + announcement[position].Subject;
            txtDetails.Text = "Details: " + announcement[position].Details;
            txtDate.Text = "Date: " + announcement[position].Date;
            imgProfile.SetImageBitmap(announcement[position].Image);
            return view;
        }
    }
}