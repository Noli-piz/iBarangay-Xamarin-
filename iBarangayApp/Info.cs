﻿using Android.App;
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
    public class Info
    {
        private static String email, username, password;
        public void Infos(String Email, String Username, String Password)
        {
            email = Email;
            username = Username;
            password = Password;
        }

        public string getStrEmail()
        {
            return email;
        }

        public string getStrUsername()
        {
            return username;
        }
        public string getStrPassword()
        {
            return password;
        }
    }
}