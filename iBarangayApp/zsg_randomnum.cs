using System;

namespace iBarangayApp
{
    public class zsg_randomnum
    {
        private static string randNum = "";
        public zsg_randomnum()
        {
            reset();
            for (int i = 0; i < 6; i++)
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