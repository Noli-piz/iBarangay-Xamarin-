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
    class csApiKey
    {

        private static string sendgridKey, sendgridEmail;

        public void loadKeys()
        {
            sendgridKey = "";
            sendgridEmail = "sti_ibarangay@outlook.com";
        }

        public string getSendGridKey()
        {
            return sendgridKey;
        }

        public string getSendGridEmail()
        {
            return sendgridEmail;
        }
    }
}