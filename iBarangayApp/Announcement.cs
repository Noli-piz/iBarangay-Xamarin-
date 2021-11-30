using Android.Graphics;
using System;
using System.Net;
using System.Threading.Tasks;

namespace iBarangayApp
{
    public class Announcement
    {
        public int id_announcement { get; set; }
        public string Date { get; set; }
        public string Subject { get; set; }

        public string Level { get; set; }
        public string ImageLocation { get; set; }
        public string Details { get; set; }

        public Bitmap Image { get; set; }

        public async void DownloadImage(String url)
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

            Image = imageBitmap;
        }
    }
}