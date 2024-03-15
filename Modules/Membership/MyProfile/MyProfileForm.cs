using Serenity.ComponentModel;
using Serenity.Data.Mapping;
using Serenity.Web;
using System;

namespace WinAuthSample.Membership.Forms
{
    [FormScript("Membership.MyProfile")]
    [BasedOnRow(typeof(MyProfileRow), CheckNames = true)]
    public class MyProfileForm
    {

        
        //public string Username { get; set; }
        [LabelWidth(200, UntilNext = true)]

        public string DisplayName { get; set; }

        [Size(4), MaxLength(4)]
        public string FormInitials { get; set; }


       // [TwoThirdWidth]
        public string Email { get; set; }

        //[OneThirdWidth]
        //public string MobilePhoneNumber { get; set; }
      

        /*
        TwoThirdWidth]
        public string SignatureName { get; set; }
        */
        

        /*
        [OneThirdWidth]
        public string SignatureTitle { get; set; }
        */
       
        

        [TwoThirdWidth]
        public string UserImage { get; set; }

        [Hidden]
        public string Username { get; set; }
     
     
   
    }
}