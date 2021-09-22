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
    public class ViewHolder3 : Java.Lang.Object
    {
        public TextView id { get; set; }
        public TextView quantity { get; set; }
        public TextView item { get; set; }
        public TextView purpose { get; set; }
        public TextView date { get; set; }
        public TextView status { get; set; }
    }

    public class CustomAdapterService : BaseAdapter
    {
        private Activity activity;
        private List<SFrag> serviceArrayList;

        public CustomAdapterService(Activity activity, List<SFrag> serviceArrayList)
        {

            this.activity = activity;
            this.serviceArrayList = serviceArrayList;

        }
        public override int Count
        {
            get
            {
                return serviceArrayList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return serviceArrayList[position].id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.lyt_service_listview, parent, false);

            var txtItem = view.FindViewById<TextView>(Resource.Id.itemname_listitem);
            var txtDate = view.FindViewById<TextView>(Resource.Id.requesteddate_listitem);
            var txtPurpose = view.FindViewById<TextView>(Resource.Id.purpose_listitem);
            var txtStatus = view.FindViewById<TextView>(Resource.Id.status_listitem);

            txtItem.Text = txtItem.Text + serviceArrayList[position].item;
            txtDate.Text = txtDate.Text + serviceArrayList[position].date;
            txtPurpose.Text = txtPurpose.Text + serviceArrayList[position].purpose;
            txtStatus.Text = txtStatus.Text + serviceArrayList[position].status;

            return view;
        }
    }
}