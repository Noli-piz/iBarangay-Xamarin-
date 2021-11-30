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