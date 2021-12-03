namespace iBarangayApp
{
    class zsg_ApiKey
    {

        private static string sendgridKey, sendgridEmail;

        public void loadKeys()
        {
            sendgridKey = "";
            sendgridEmail = "sti_ibarangay@outlook.com";
        }

        public void setSendGridKey(string key)
        {
            sendgridKey = key;
        }


        public void setSendGridEmail(string email)
        {
            sendgridEmail = email;
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