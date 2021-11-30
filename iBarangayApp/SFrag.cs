using System;

namespace iBarangayApp
{
    public class SFrag
    {
        public int id;
        public String item, quantity, purpose, date, status;
        public SFrag(int id, String item, String quantity, String date, String purpose, String status)
        {
            this.id = id;
            this.quantity = quantity;
            this.item = item;
            this.purpose = purpose;
            this.date = date;
            this.status = status;
        }
    }
}