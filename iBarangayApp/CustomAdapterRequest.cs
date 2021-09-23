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

namespace iBarangayApp
{
    public class ViewHolder2 : Java.Lang.Object
    {
        public TextView id { get; set; }
        public TextView item { get; set; }
        public TextView purpose { get; set; }
        public TextView date { get; set; }
        public TextView status { get; set; }
    }

    public class CustomAdapterRequest : BaseAdapter
    {
        private Activity activity;
        private List<RFrag> requestArrayList;

        public CustomAdapterRequest(Activity activity, List<RFrag> requestArrayList)
        {

            this.activity = activity;
            this.requestArrayList = requestArrayList;

        }
        public override int Count
        {
            get
            {
                return requestArrayList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return requestArrayList[position].id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.lyt_request_listview, parent, false);

            var txtItem = view.FindViewById<TextView>(Resource.Id.itemname_listitem);
            var txtDate = view.FindViewById<TextView>(Resource.Id.requesteddate_listitem);
            var txtPurpose = view.FindViewById<TextView>(Resource.Id.purpose_listitem);
            var txtStatus = view.FindViewById<TextView>(Resource.Id.status_listitem);

            txtItem.Text = "Document: " + requestArrayList[position].item;
            txtDate.Text = "Requested Date: " + requestArrayList[position].date;
            txtPurpose.Text = "Purpose: " + requestArrayList[position].purpose;
            txtStatus.Text = "Status: " + requestArrayList[position].status;

            return view;
        }
    }
}