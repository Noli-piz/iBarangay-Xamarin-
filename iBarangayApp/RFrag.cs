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
    public class RFrag
    {
        public int id;
        public String item, purpose, date, status;
        public RFrag(int id,String item, String date, String purpose, String status)
        {
            this.id = id;
            this.item = item;
            this.purpose = purpose;
            this.date = date;
            this.status = status;
        }
    }
}