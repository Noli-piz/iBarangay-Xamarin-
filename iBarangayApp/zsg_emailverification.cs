using Org.Json;
using System.Net.Http;

namespace iBarangayApp
{
    public class zsg_emailverfication
    {
        private static string email = "", username="";

        public void setEmail(string eml)
        {
            email = eml;
        }

        public string getEmail()
        {
            return email;
        }

        public void setUsername(string usr)
        {
            username = usr;
        }

        public string getUsername()
        {
            return username;
        }

        public void reset()
        {
            email = "";
            username = "";
        }


    }
}