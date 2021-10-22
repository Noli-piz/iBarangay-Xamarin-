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
    public class zsg_hosting
    {

        private static String hosting1 = "http://192.168.254.114/iBarangay/";
        private static String hosting2 = "https://php1002001.000webhostapp.com/";

        private static String announcement, login, requestall, requestpending, requestapproved, requestdisapproved, requestdelivered, requestcancelled;
        private static String insertrequest, certificate, deliveryoption, insertservice, itemquantity;
        private static String barangayitems, checkusername, signup3, civilstatus, gender, purok, personalinfo,updatepersonalinfo;
        private static String serviceall, servicepending, serviceapproved, servicedisapproved, serviceborrowed, servicereturned;
        private static String insertverification;
        public zsg_hosting()
        {
            String hosting = hosting2;

            announcement = hosting + "ibarangay_announcement.php";          //Announcement
            login = hosting + "ibarangay_login.php";                        //Login
            requestall = hosting + "ibarangay_requestall.php";              //Req_Fragment1  // ?Username=
            requestpending = hosting + "ibarangay_requestpending.php";      //Req_Fragment2  // ?Username=
            requestapproved = hosting + "ibarangay_requestapproved.php";     //Req_Fragment3  // ?Username=
            requestdisapproved = hosting + "ibarangay_requestdisapproved.php";  //Req_Fragment4  // ?Username=
            requestdelivered = hosting + "ibarangay_requestdelivered.php";  //Req_Fragment5  // ?Username=
            requestcancelled = hosting + "ibarangay_requestcancelled.php";  //Req_Fragment6  // ?Username=
            insertrequest = hosting + "ibarangay_insertrequest.php";        //RequestModule
            certificate = hosting + "ibarangay_certificate.php";            //RequestModule
            deliveryoption = hosting + "ibarangay_deliveryoptions.php";     //RequestModule
            insertservice = hosting + "ibarangay_insertservice.php";        //ServiceModule
            itemquantity = hosting + "ibarangay_itemquantity.php";          //ServiceModule //?ItemName=
            barangayitems = hosting + "ibarangay_items.php";                //ServiceModule
                                                                            //deliveryoption = hosting + "ibarangay_deliveryoptions.php";     //ServiceModule
            checkusername = hosting + "ibarangay_checkusername.php";        //SignUp
            signup3 = hosting + "ibarangay_signup3.php";                    //SignUp2
            civilstatus = hosting + "ibarangay_civilstatus.php";            //SignUp2
            gender = hosting + "ibarangay_gender.php";                      //SignUp2
            purok = hosting + "ibarangay_purok.php";                        //SignUp2
            personalinfo = hosting + "ibarangay_personalinfo.php";          //zsgNameandImage
            updatepersonalinfo = hosting + "ibarangay_updateinformation.php"; //UpdateInfo Module
            serviceall = hosting + "ibarangay_serviceall.php";              //Ser_Fragment1  // ?Username=
            servicepending = hosting + "ibarangay_servicepending.php";      //Ser_Fragment2  // ?Username=
            serviceapproved = hosting + "ibarangay_serviceapproved.php";     //Ser_Fragment3  // ?Username=
            servicedisapproved = hosting + "ibarangay_servicedisapproved.php";  //Ser_Fragment4  // ?Username=
            serviceborrowed = hosting + "ibarangay_servicebarrowed.php";  //Ser_Fragment5  // ?Username=
            servicereturned = hosting + "ibarangay_servicereturned.php";  //Ser_Fragment6  // ?Username=

            insertverification = hosting + "ibarangay_insertverification.php";  //VerifyAccount2
        }

        public String getAnnouncement()
        {
            return announcement;
        }

        public String getLogin()
        {
            return login;
        }

        public String getRequestall()
        {
            return requestall;
        }

        public String getRequestapproved()
        {
            return requestapproved;
        }

        public String getRequestdisapproved()
        {
            return requestdisapproved;
        }

        public String getRequestpending()
        {
            return requestpending;
        }

        public String getInsertrequest()
        {
            return insertrequest;
        }

        public String getCertificate()
        {
            return certificate;
        }

        public String getDeliveryoption()
        {
            return deliveryoption;
        }

        public String getInsertservice()
        {
            return insertservice;
        }

        public String getItemquantity()
        {
            return itemquantity;
        }

        public String getBarangayitems()
        {
            return barangayitems;
        }

        public String getCheckusername()
        {
            return checkusername;
        }

        public String getSignup3()
        {
            return signup3;
        }

/*        public String getHosting()
        {
            return hosting;
        }*/

        public String getCivilstatus()
        {
            return civilstatus;
        }

        public String getGender()
        {
            return gender;
        }

        public String getPurok()
        {
            return purok;
        }

        public String getPersonalinfo()
        {
            return personalinfo;
        }

        public String getUpdatePersonalinfo()
        {
            return updatepersonalinfo;
        }

        public String getVerification()
        {
            return insertverification;
        }

        public String getServiceall()
        {
            return serviceall;
        }

        public String getServicepending()
        {
            return servicepending;
        }

        public String getServiceapproved()
        {
            return serviceapproved;
        }

        public String getServicedisapproved()
        {
            return servicedisapproved;
        }

        public String getRequestdelivered()
        {
            return requestdelivered;
        }

        public String getRequestcancelled()
        {
            return requestcancelled;
        }

        public String getServiceborrowed()
        {
            return serviceborrowed;
        }

        public String getServicereturned()
        {
            return servicereturned;
        }
    }
}