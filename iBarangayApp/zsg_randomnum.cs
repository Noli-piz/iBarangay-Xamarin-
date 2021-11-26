using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace iBarangayApp
{
    public class zsg_randomnum
    {
        private static string randNum = "";
        public zsg_randomnum()
        {
            reset();
            for (int i =0; i<6 ;i++)
            {
                randNum += new Random().Next(1, 9).ToString();
            }
        }

        public string randomNum()
        {
            return randNum;
        }

        public void reset()
        {
            randNum = "";
        }
    }
}