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
    public class zsg_nameandimage
    {
        private static String strusername, strname, stremail, strImg;
        private static Bitmap bitImg;

        private zsg_hosting sg_host = new zsg_hosting();
        public void nameandimage()
        {
            RetrieveInfo();
        }

        public void reset()
        {
            stremail = "";
            strusername = "";
            strname = "";
            stremail = "";
            strImg = "";
            bitImg = null;
        }

        private async void RetrieveInfo()
        {
            using (var client = new HttpClient())
            {
                zsg_hosting hosting = new zsg_hosting();
                var uri = hosting.getPersonalinfo() + "?Username=" + strusername;
                var result = await client.GetStringAsync(uri);

                JSONObject jsonresult = new JSONObject(result);
                int success = jsonresult.GetInt("success");

                if (success == 1)
                {
                    JSONArray information = jsonresult.GetJSONArray("info");
                    String Fname, Mname, Lname, Sname, Image;

                    JSONObject info = information.GetJSONObject(0);
                    Fname = info.GetString("Fname");
                    Mname = info.GetString("Mname");
                    Lname = info.GetString("Lname");
                    Sname = info.GetString("Sname");
                    Image = info.GetString("Image");

                    strname = Fname + " " + Mname + " " + Lname + " " + Sname;
                    strImg = Image;
                    DownloadImage(Image);
                }

            }
        }

        private async void DownloadImage(string url)
        {
            try
            {
                Bitmap imageBitmap = null;
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

                bitImg = imageBitmap;
            }
            catch (Exception e)
            {
                bitImg = null;
            }
        }


        //Setters Getters
        public String getStrusername()
        {
            return strusername;
        }

        public void setStrusername(String strusernames)
        {
            strusername = strusernames;
        }

        public String getStrname()
        {
            return strname;
        }

        public String getStremail()
        {
            return stremail;
        }

        public String getStrImg()
        {
            return strImg;
        }

        public Bitmap getImg()
        {
            return bitImg;
        }
    }
}