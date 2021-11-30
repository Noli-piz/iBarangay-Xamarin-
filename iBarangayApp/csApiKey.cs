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