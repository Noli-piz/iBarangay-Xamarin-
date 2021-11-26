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
    public class zsg_emailverfication
    {
        private static string email = "";

        public void setEmail(string eml)
        {
           email = eml;
        }

        public string getEmail()
        {
            return email;
        }

        public void reset()
        {
            email = "";
        }
    }
}