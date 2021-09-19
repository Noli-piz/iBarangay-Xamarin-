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
    public class  Announcement
    {
        public  int id_announcement { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public string ImageLocation { get; set; }
        public string Details { get; set; }
    }
}