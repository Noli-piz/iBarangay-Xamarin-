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
using static iBarangayApp.MainAnnouncement;

namespace iBarangayApp
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView id_announcement { get; set; }
        public TextView Date { get; set; }
        public TextView Subject { get; set; }
        public TextView Level { get; set; }
        public TextView ImageLocation { get; set; }
        public TextView Details { get; set; }
    }

    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Announcement> announcement;

        public CustomAdapter(Activity activity, List<Announcement> announcement) {

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

            txtSubject.Text = announcement[position].Subject;
            txtDetails.Text = announcement[position].Details;
            txtDate.Text = announcement[position].Date;

            return view;
        }
    }
}